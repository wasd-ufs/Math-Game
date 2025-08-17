using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controla a física do personagem quando ele está no ar
/// Faz parte da máquina de estados do personagem
/// </summary>
public class AirborneState : MovementState
{
    private void Awake()
    {
        SetContext(GetComponent<MovementFSM>());
    }

    public override void RunInUpdate()
    {
        //fazer nada
    }

    public override void Movement()
    {
        _context.isGrounded = VerifyIfIsGrounded();
        
        if (_context.isGrounded)
            _context.TransitionTo(_context.groundedState);
        
        //Movimento Horizontal
        _context.velocity.x = Mathf.MoveTowards(_context.velocity.x,
            _context.MovementInputVector.x * _context.maxMoveSpeed, _context.horizontalAcceleration * Time.fixedDeltaTime);
        
        //Movimento Vertical/Gravidade
        bool isFalling = _context.velocity.y < 0 || !_context.isJumpKeyBeingPressed;
        float gravityMultiplier = isFalling ? _context.jumpGravityMultiplier : 1f;
        _context.velocity.y += _context.gravity * gravityMultiplier * Time.deltaTime;
        _context.velocity.y = Mathf.Max(_context.velocity.y, _context.terminalVelocity);
        
        //Corrigir posição
        Vector2 position = _context.rigidbody.position;
        position += _context.velocity * Time.fixedDeltaTime;
         
        //Aplicar posição ao RigidBody
        _context.rigidbody.MovePosition(position);
    }
}
