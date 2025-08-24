using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Gerencia todas as cameras do Cinemachine na cena.
/// Responsável por controlar a amortecimento, o movimento panorâmico (pan), a troca entre diferentes cameras e o autoScroll.
/// </summary>
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera[] _allCinemachineCameras;

    [Header("Y damping")]
    [SerializeField] private float _fallPanAmount = 0.35f;
    [SerializeField] private float _normalPanAmount = 1f;
    [SerializeField] private float _fallPanTime = 0.35f;
    [SerializeField] public float _fallSpeedDampingChangeThreshold = -15f;

    [Header("AutoScroll Settings")]
    [SerializeField] private CinemachineCamera _autoScrollCamera;
    [SerializeField] private CameraAutoScrollTarget _cameraAutoScrollTarget;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpPanCoroutine;
    private Coroutine _panCameraCoroutine;

    private CinemachineCamera _currentCamera;
    private CinemachinePositionComposer _positionComposer;

    private Vector3 _startingTrackedObjectOffset;

    public CinemachinePositionComposer CurrentPositionComposer => _positionComposer;

    // Configura o singleton e inicializa a camera ativa.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateCurrentCamera();

        if (_currentCamera != null)
        {
            _positionComposer = _currentCamera.GetComponentInChildren<CinemachinePositionComposer>();
        }

        if (_positionComposer != null)
        {
            _startingTrackedObjectOffset = _positionComposer.TargetOffset;
        }
        else
        {
            _startingTrackedObjectOffset = Vector3.zero;
        }
    }

    // Seleciona a camera com maior prioridade.
    private void UpdateCurrentCamera()
    {
        _currentCamera = null;
        int highestPriority = int.MinValue;

        foreach (var camera in _allCinemachineCameras)
        {
            if (camera != null && camera.Priority > highestPriority)
            {
                highestPriority = camera.Priority;
                _currentCamera = camera;
            }
        }
    }

    #region Y Damping
    // Inicia interpolação do damping no eixo Y.
    public void LerpYDamping(bool isPlayerFalling)
    {
        if (_lerpPanCoroutine != null) StopCoroutine(_lerpPanCoroutine);
        _lerpPanCoroutine = StartCoroutine(LerpAction(isPlayerFalling));
    }

    // Interpola suavemente o damping no eixo Y.
    private IEnumerator LerpAction(bool isPlayerFalling)
    {
        if (_positionComposer == null) yield break;

        var dampingInterface = _positionComposer as CinemachineFreeLookModifier.IModifiablePositionDamping;
        if (dampingInterface == null) yield break;

        IsLerpingYDamping = true;

        float startDampAmount = dampingInterface.PositionDamping.y;
        float endDampAmount = isPlayerFalling ? _fallPanAmount : _normalPanAmount;

        if (isPlayerFalling) LerpedFromPlayerFalling = true;

        float elapsedTime = 0f;
        while (elapsedTime < _fallPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallPanTime);
            dampingInterface.PositionDamping = new Vector3(
                dampingInterface.PositionDamping.x,
                lerpedPanAmount,
                dampingInterface.PositionDamping.z
            );
            yield return null;
        }

        IsLerpingYDamping = false;
    }
    #endregion

    public enum PanDirection { Up, Down, Left, Right }

    #region Pan Camera
    // Inicia o movimento de panning da camera.
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        if (_panCameraCoroutine != null) StopCoroutine(_panCameraCoroutine);
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    // Realiza o panning suave da camera.
    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        if (_positionComposer == null) yield break;

        Vector3 endPos = Vector3.zero;
        Vector3 startingPos = Vector3.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up: endPos = Vector3.up; break;
                case PanDirection.Down: endPos = Vector3.down; break;
                case PanDirection.Left: endPos = Vector3.left; break;
                case PanDirection.Right: endPos = Vector3.right; break;
            }

            endPos *= panDistance;
            startingPos = _positionComposer.TargetOffset;
            endPos += startingPos;
        }
        else
        {
            startingPos = _positionComposer.TargetOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / panTime);
            _positionComposer.TargetOffset = panLerp;
            yield return null;
        }

        _positionComposer.TargetOffset = endPos;
    }
    #endregion

    #region Swap Camera
    // Troca entre cameras com base na direção de saída do jogador.
    public void SwapCamera(CinemachineCamera camLeft, CinemachineCamera camRight, Vector2 exitDirection)
    {
        if (_currentCamera == camLeft && exitDirection.x > 0f)
        {
            camLeft.enabled = false;
            camRight.enabled = true;
            _currentCamera = camRight;
        }
        else if (_currentCamera == camRight && exitDirection.x < 0f)
        {
            camRight.enabled = false;
            camLeft.enabled = true;
            _currentCamera = camLeft;
        }
        else
        {
            return;
        }

        _positionComposer = _currentCamera.GetComponentInChildren<CinemachinePositionComposer>();
        if (_positionComposer != null)
        {
            _startingTrackedObjectOffset = _positionComposer.TargetOffset;
        }
    }
    #endregion

    #region Auto Scroll Control
    
    // Inicia o modo de auto-scroll com parâmetros específicos.
    public void StartAutoScroll(Vector3 startPosition, Vector2 direction, float speed)
    {
        _cameraAutoScrollTarget.SetPosition(startPosition);

        _cameraAutoScrollTarget.StartScrolling(direction, speed);
    }

    /// Para o modo de auto-scroll e retorna para a câmera do jogador.
    public void StopAutoScroll()
    {
        if (_autoScrollCamera == null || _cameraAutoScrollTarget == null) return;
        
        // Usamos o novo método para parar
        _cameraAutoScrollTarget.StopScrolling();
    }
    #endregion
}