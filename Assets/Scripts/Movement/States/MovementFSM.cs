using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// "Context" da implementação segundo padrão State da movimentaão do personagem
/// Gerencia em qual estado a máquina está, além das variáveis de física e algumas chamadas de movimentação 
/// </summary>
public class MovementFSM : MonoBehaviour
{
    [SerializeField]private MovementState _movementState;
    
    [HideInInspector]public Rigidbody2D rigidbody;
    [HideInInspector]public InputSystem_Actions _inputSystem;
    [HideInInspector]public GroundedState groundedState;
    [HideInInspector]public AirborneState airborneState;

    [HideInInspector] public Vector2 MovementInputVector;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isJumpKeyBeingPressed;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isSprinting;

    public Vector2 velocity;

    public float sprintSpeedModifier = 1.5f;
    public float jumpGravityMultiplier = 2f;
    public float maxMoveSpeed = 7f;
    public float horizontalAcceleration = 32f;
    public float terminalVelocity = -40f;
    public float jumpForce = 20f;
    public float gravity = -80f;

    public void TransitionTo(MovementState newState)
    {
        _movementState = newState;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Enable();
        _inputSystem.Player.Jump.performed += JumpPerformed;
        _inputSystem.Player.Move.performed += MovePerformed;
        _inputSystem.Player.Sprint.performed += SprintPerformed;

        airborneState = GetComponent<AirborneState>();
        groundedState = GetComponent<GroundedState>();
        
        TransitionTo(airborneState);
    }
    
    private void Update()
    {
        MovementInputVector = _inputSystem.Player.Move.ReadValue<Vector2>();
        isJumpKeyBeingPressed = _inputSystem.Player.Jump.ReadValue<float>() > 0;
        isSprinting = _inputSystem.Player.Sprint.ReadValue<float>() > 0;
        
        _movementState.RunInUpdate();
    }

    private void FixedUpdate()
    {
        _movementState.Movement();
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        _movementState.JumpPerformed(context);
    }

    private void MovePerformed(InputAction.CallbackContext context)
    {
        _movementState.MovePerformed(context);
    }

    private void SprintPerformed(InputAction.CallbackContext context)
    {
        _movementState.SprintPerformed(context);
    }
}
