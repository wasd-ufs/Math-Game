using UnityEngine;

/// <summary>
/// Controla um objeto para se mover constantemente em uma direção e velocidade.
/// Este objeto servirá como o Follow Target para uma Camera AutoScroll.
/// </summary>
public class CameraAutoScrollTarget : MonoBehaviour
{
    private float _scrollSpeed = 2f;
    private Vector2 _scrollDirection = Vector2.right;

    private bool _isScrolling = false;

    void Update()
    {
        if (_isScrolling)
        {
            transform.position += (Vector3)_scrollDirection.normalized * _scrollSpeed * Time.deltaTime;
        }
    }
    
    /// Configura os parâmetros e inicia o scroll.
    public void StartScrolling(Vector2 direction, float speed)
    {
        _scrollDirection = direction;
        _scrollSpeed = speed;
        _isScrolling = true;
    }

    /// Para o movimento de auto-scroll.
    public void StopScrolling()
    {
        _isScrolling = false;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}