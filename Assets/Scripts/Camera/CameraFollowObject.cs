using System.Collections;
using UnityEngine;

/// <summary>
/// Controla o objeto que a camera segue, sincronizando sua posição com o jogador e gerenciando a rotação ao virar.
/// </summary>
public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Field Rotation Stats")]
    [SerializeField] private float _flipRotationTime = 0.5f;

    private Coroutine turnCoroutine;
    private bool _isFacingRight;

    // Define a direção inicial do objeto como voltada para a direita.
    void Awake()
    {
        _isFacingRight = true;
    }

    // Atualiza a posição do objeto para seguir o jogador.
    void Update()
    {
        if (_playerTransform != null)
            transform.position = _playerTransform.position;
    }

    // Inicia a coroutine para girar o objeto.
    public void CallTurn()
    {
        if (turnCoroutine != null)
            StopCoroutine(turnCoroutine);

        turnCoroutine = StartCoroutine(FlipPlayer());
    }

    // Realiza a rotação suave do objeto no eixo Y.
    private IEnumerator FlipPlayer()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _flipRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, endRotationAmount, 0f);
    }

    // Determina o ângulo final de rotação com base na direção.
    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        return _isFacingRight ? 0f : 180f;
    }
}