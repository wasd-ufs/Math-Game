using System.Collections;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Field Rotation Stats")]
    [SerializeField] private float _flipRotationTime = 0.5f;

    private Coroutine turnCoroutine;
    private bool _isFacingRight;

    void Awake()
    {
        _isFacingRight = true; // padrão inicial
    }

    void Update()
    {
        // Segue a posição do player
        if (_playerTransform != null)
            transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        if (turnCoroutine != null)
            StopCoroutine(turnCoroutine);

        turnCoroutine = StartCoroutine(FlipPlayer());
    }

    private IEnumerator FlipPlayer()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _flipRotationTime)
        {
            elapsedTime += Time.deltaTime;

            // Lerp da rotação Y
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }

        // Garante o ângulo final exato
        transform.rotation = Quaternion.Euler(0f, endRotationAmount, 0f);
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
