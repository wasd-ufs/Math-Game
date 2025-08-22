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
    [SerializeField] private CameraFollowObject _cameraFollowObject;
    
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
    public bool isFacingRight = true;

    private float _fallSpeedYDampingChangeThreshold;

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

        if (CameraManager.Instance != null)
        {
            _fallSpeedYDampingChangeThreshold = CameraManager.Instance._fallSpeedDampingChangeThreshold;
        }
    }
    
    private void Update()
    {
        MovementInputVector = _inputSystem.Player.Move.ReadValue<Vector2>();
        isJumpKeyBeingPressed = _inputSystem.Player.Jump.ReadValue<float>() > 0;
        isSprinting = _inputSystem.Player.Sprint.ReadValue<float>() > 0;
        
        if (MovementInputVector.x > 0 && !isFacingRight)
            Turn();
        else if (MovementInputVector.x < 0 && isFacingRight)
            Turn();
        
        _movementState.RunInUpdate();

        if (velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }

        if (velocity.y >= 0 && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpedFromPlayerFalling = false;
            CameraManager.Instance.LerpYDamping(false);
        }
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
    
    private void Turn()
    {
        isFacingRight = !isFacingRight;

        Vector3 rotator = new Vector3(0f, isFacingRight ? 0f : 180f, 0f);
        transform.rotation = Quaternion.Euler(rotator);

        if (_cameraFollowObject != null)
            _cameraFollowObject.CallTurn();
    }
}
