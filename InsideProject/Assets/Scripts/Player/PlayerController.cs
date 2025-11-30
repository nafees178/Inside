using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public LayerMask groundMask;

    [Header("Camera Position")]
    public Vector3 cameraLocalOffset = new Vector3(0f, 0.8f, 0f);

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Look")]
    public float mouseSensitivity = 60f;
    public float verticalLookLimit = 85f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 1.4f;
    public float gravity = -9.81f;

    [Header("Hover Settings")]
    public bool useHover = true;
    public float hoverHeight = 0.3f;
    public float hoverRayLength = 2f;
    public float hoverLerpSpeed = 20f;
    public float hoverGroundTolerance = 0.05f;

    public enum InputSource { Legacy, NewInputSystem }

    private IPlayerInput _input;
    private StateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }

    [HideInInspector] public bool IsGrounded;
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool IsRunning;
    [HideInInspector] public Vector3 inputDirection;
    [HideInInspector] public Vector3 velocity;

    [HideInInspector] public CharacterController controller;

    private float _cameraPitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        SetupInput();

        _stateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this, _stateMachine);
        MoveState = new PlayerMoveState(this, _stateMachine);
        JumpState = new PlayerJumpState(this, _stateMachine);
        AirState = new PlayerAirState(this, _stateMachine);

        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = cameraLocalOffset;
        playerCamera.transform.localRotation = Quaternion.identity;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        ReadInput();
        LookInput();

        _stateMachine.CurrentState?.HandleInput();
        _stateMachine.CurrentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState?.PhysicsUpdate();
    }

    private void SetupInput()
    {
         _input = new LegacyInput();
    }

    private void ReadInput()
    {
        Vector2 move = _input.Move;
        IsRunning = _input.RunHeld;
        JumpPressed = _input.JumpPressed;

        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        right.Normalize();

        inputDirection = (forward * move.y + right * move.x).normalized;
    }

    private void LookInput()
    {
        Vector2 look = _input.Look;

        float yaw = look.x * mouseSensitivity * Time.deltaTime;
        float pitch = look.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * yaw);

        _cameraPitch -= pitch;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
    }

    public bool TryGetHoverGround(out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * 0.2f;
        return Physics.Raycast(origin, Vector3.down, out hit, hoverRayLength, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.2f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + Vector3.down * hoverRayLength);
    }
}
