using UnityEngine;

/// <summary>
/// Classe de extenções para tipos da Unity
/// Implementa funções úteis, inicialmente usada na movimentação do personagem  
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Usa um circleCast na borda do GameObject que o chama para saber se está colidindo
    /// com algo de um layer específico.
    /// </summary>
    /// <param name="rigidbody"> Chamado com notação de ponto "rb.Raycast"</param>
    /// <param name="direction"> Direção na qual se quer verificar a colisão</param>
    /// <param name="layerMask"> Layer que deve se verificar a colisão</param>
    /// <returns></returns>
    public static bool CircleCastCheck(this Rigidbody2D rigidbody, Vector2 direction, LayerMask layerMask)
    {
        if (rigidbody.isKinematic) {
            return false;
        }

        Vector2 edge = rigidbody.ClosestPoint(rigidbody.position + direction);
        float radius = (edge - rigidbody.position).magnitude / 2f;
        float distance = radius + 0.125f;

        Vector2 point = rigidbody.position + (direction.normalized * distance);
        Collider2D collider = Physics2D.OverlapCircle(point, radius, layerMask);
        return collider != null && collider.attachedRigidbody != rigidbody;
    }

    /// <summary>
    /// Método que usa produto escalar para verificar se o vetor direção de dois objetos aponta para uma direção dada.
    /// </summary>
    /// <param name="transform"> Objeto referência, chamado com notação de ponto</param>
    /// <param name="target">Transform do segundo objeto</param>
    /// <param name="testDirection">Direção de teste</param>
    /// <returns></returns>
    public static bool TestFacingDirection(this Transform transform, Transform target, Vector2 testDirection)
    {
        Vector2 direction = target.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.75f;
    }
}
