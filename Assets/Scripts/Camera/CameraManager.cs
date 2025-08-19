using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera[] _allCinemachineCameras;

    [Header("Controls for lerping the Y damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.35f;
    [SerializeField] private float _normalPanAmount = 1f;
    [SerializeField] private float _fallPanTime = 0.35f;
    [SerializeField] public float _fallSpeedDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpPanCoroutine;
    private CinemachineCamera _currentCamera;
    private CinemachinePositionComposer _positionComposer;

    private void Awake()
    {
        // Configuração do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Encontrar a câmera ativa baseada em prioridade
        UpdateCurrentCamera();
    }

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

        // Atualizar o Position Composer se uma câmera válida for encontrada
        if (_currentCamera != null)
        {
            _positionComposer = _currentCamera.GetComponentInChildren<CinemachinePositionComposer>();
            if (_positionComposer == null)
            {
                Debug.LogWarning("Nenhum CinemachinePositionComposer encontrado na câmera atual. Certifique-se de adicionar o componente 'Body > Position Composer' na CinemachineCamera.");
            }
        }
        else
        {
            Debug.LogWarning("Nenhuma câmera ativa encontrada no array _allCinemachineCameras.");
        }
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        // Parar qualquer coroutine de lerp existente para evitar sobreposições
        if (_lerpPanCoroutine != null)
        {
            StopCoroutine(_lerpPanCoroutine);
        }
        _lerpPanCoroutine = StartCoroutine(LerpAction(isPlayerFalling));
    }

    private IEnumerator LerpAction(bool isPlayerFalling)
    {
        if (_positionComposer == null)
        {
            Debug.LogWarning("Não é possível interpolar o Y damping: Position Composer não encontrado.");
            yield break;
        }

        var dampingInterface = _positionComposer as CinemachineFreeLookModifier.IModifiablePositionDamping;
        if (dampingInterface == null)
        {
            Debug.LogWarning("Interface IModifiablePositionDamping não suportada no Position Composer.");
            yield break;
        }

        IsLerpingYDamping = true;

        // Capturar o valor inicial de Y damping
        float startDampAmount = dampingInterface.PositionDamping.y;
        float endDampAmount = isPlayerFalling ? _fallPanAmount : _normalPanAmount;

        if (isPlayerFalling)
        {
            LerpedFromPlayerFalling = true;
        }

        // Interpolar o valor de Y damping
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

    // Opcional: Chame isso para atualizar a câmera ativa se as prioridades mudarem
    public void RefreshActiveCamera()
    {
        UpdateCurrentCamera();
    }
}