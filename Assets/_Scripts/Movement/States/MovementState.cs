using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe "State" da máquina de esdatos de movimentação do personagem 
/// </summary>
public abstract class MovementState : MonoBehaviour
{
    protected MovementFSM _context;

    public void SetContext(MovementFSM context)
    {
        _context = context;
    }

    protected bool VerifyIfIsGrounded()
    {
        return _context.rigidbody.CircleCastCheck(Vector2.down, LayerMask.GetMask("Ground"));
    }

    public abstract void RunInUpdate();

    public abstract void Movement();
    
    /// <summary>
    /// Chamada pela máquina quando ação "Move" é realizada
    /// (Chamada apenas quando o jogador primeiro aperta o botão) 
    /// </summary>
    /// <param name="context">Retorno do evento ao qual a função está inscrita</param>
    public virtual void MovePerformed(InputAction.CallbackContext context) { }
    
    /// <summary>
    /// Chamada pela máquina quando ação "Jump" é realizada
    /// (Chamada apenas quando o jogador primeiro aperta o botão) 
    /// </summary>
    /// <param name="context">Retorno do evento ao qual a função está inscrita</param>
    public virtual void JumpPerformed(InputAction.CallbackContext context) { }

    /// <summary>
    /// Chamada pela máquina quando ação "Sprint" é realizada
    /// (Chamada apenas quando o jogador primeiro aperta o botão) 
    /// </summary>
    /// <param name="context">Retorno do evento ao qual a função está inscrita</param>
    public virtual void SprintPerformed(InputAction.CallbackContext context) { }
}
