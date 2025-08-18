using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla a física do personagem quando ele está no chão
/// Faz parte da máquina de estados do personagem
/// </summary>
public class GroundedState : MovementState
{
    private void Awake()
    {
        SetContext(GetComponent<MovementFSM>());
    }

    public override void RunInUpdate()
    {
        //Lógica de Pulo
        _context.isJumping = false;

        if (_context._inputSystem.Player.Jump.triggered)
        {
            _context.velocity.y = _context.JumpForce;
            _context.isJumping = true;
        }
    }

    public override void Movement()
    {
        _context.isGrounded = VerifyIfIsGrounded();
        
        if (!_context.isGrounded)
            _context.TransitionTo(_context.airborneState);
        
        _context.velocity.y = Mathf.Max(0f,  _context.velocity.y);
        
        //Movimento Horizontal
        float horizontalSpeedModifier = _context.isSprinting ? _context.SprintSpeedModifier : 1f;
        _context.velocity.x = Mathf.MoveTowards(_context.velocity.x,
            _context.MovementInputVector.x * _context.MaxMoveSpeed * horizontalSpeedModifier,
            _context.HorizontalAcceleration * Time.fixedDeltaTime);
        
        //Movimento Vertical/Gravidade
        bool isFalling = _context.velocity.y < 0 || !_context.isJumpKeyBeingPressed;
        float gravityMultiplier = isFalling ? _context.JumpGravityMultiplier : 1f;
        _context.velocity.y += _context.Gravity * gravityMultiplier * Time.deltaTime;
        _context.velocity.y = Mathf.Max(_context.velocity.y, _context.TerminalVelocity);
        
        Vector2 position = _context.rigidbody.position;
        position += _context.velocity * Time.fixedDeltaTime;
         
        _context.rigidbody.MovePosition(position);
    }
    
}