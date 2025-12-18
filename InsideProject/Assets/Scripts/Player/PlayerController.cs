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
    public float coyoteTime = 0.1f;
    public float gravity = -30f;

    [Header("Jump Feel")]
    public float fallGravityMultiplier = 2.8f;
    public float lowJumpGravityMultiplier = 2.2f;


    [Header("Hover Settings")]
    public bool useHover = true;
    public float hoverHeight = 0.3f;
    public float hoverRayLength = 2f;
    public float hoverSphereRadiusMultiplier = 0.9f;
    public float hoverLerpSpeed = 20f;
    public float hoverGroundTolerance = 0.05f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Push Settings")]
    public bool enablePushing = true;
    public float pushForce = 5f;
    public LayerMask pushableLayers;
    public bool onlyPushOnGround = true;



    public float lastDashTime = -999f;
    [HideInInspector] public CameraEffects cameraEffects;

    public IPlayerInput _input;
    private StateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    
    public bool IsGrounded;
    public float lastGroundedTime;
    public bool canDash;
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool IsRunning;
    [HideInInspector] public bool DashPressed;
    [HideInInspector] public Vector3 inputDirection;
    [HideInInspector] public Vector3 velocity;



    [HideInInspector] public CharacterController controller;

    public enum Controller
    {
        Player,
        Ghost
    }

    public Controller gameObjectController = Controller.Player;

    private float _cameraPitch;

    private void Awake()
    {
        SetupInput();
        controller = GetComponent<CharacterController>();

        _stateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this, _stateMachine);
        MoveState = new PlayerMoveState(this, _stateMachine);
        JumpState = new PlayerJumpState(this, _stateMachine);
        AirState = new PlayerAirState(this, _stateMachine);
        DashState = new PlayerDashState(this, _stateMachine);


        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = cameraLocalOffset;
        playerCamera.transform.localRotation = Quaternion.identity;
        cameraEffects = playerCamera.GetComponent<CameraEffects>();

    }

    private void Start()
    {
        _stateMachine.Initialize(IdleState);
        playerCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

    }

    private void Update()
    {
        ReadInput();
        LookInput();
        canDash = CanDash();
        _stateMachine.CurrentState?.HandleInput();
        _stateMachine.CurrentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState?.PhysicsUpdate();
    }

    private void SetupInput()
    {
        switch (gameObjectController)
        {
            case Controller.Player:
                _input = GetComponent<LegacyInput>();
                break;
            case Controller.Ghost:
                _input = GetComponent<PlaybackInput>();
                break;
            default:
                break;
        }
        
    }

    private void ReadInput()
    {
        Vector2 move = _input.Move;
        IsRunning = _input.RunHeld;
        JumpPressed = _input.JumpPressed;
        DashPressed = _input.DashPressed;

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

    public void LaunchFromExternal(Vector3 launchDirection, float launchHeight)
    {
        if (launchDirection.sqrMagnitude > 0.0001f)
            launchDirection.Normalize();
        else
            launchDirection = Vector3.up;

        float verticalVelocity = Mathf.Sqrt(launchHeight * -2f * gravity);

        Vector3 horizontalDir = new Vector3(launchDirection.x, 0f, launchDirection.z).normalized;

        float horizontalSpeed = launchHeight;

        velocity = new Vector3(
            horizontalDir.x * horizontalSpeed,
            verticalVelocity,
            horizontalDir.z * horizontalSpeed
        );

        IsGrounded = false;

        _stateMachine.ChangeState(AirState);

        if (cameraEffects != null)
        {
            cameraEffects.Shake(0.2f, 0.15f);
            cameraEffects.OnDash(launchDirection);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!enablePushing)
            return;

        Rigidbody rb = hit.rigidbody;
        if (rb == null || rb.isKinematic)
            return;

        if (pushableLayers.value != 0)
        {
            if ((pushableLayers.value & (1 << hit.gameObject.layer)) == 0)
                return;
        }

        if (onlyPushOnGround && !IsGrounded)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        if (pushDir.sqrMagnitude < 0.0001f)
            return;

        pushDir.Normalize();
        rb.AddForce(pushDir * pushForce, ForceMode.VelocityChange);
    }



    public bool CanDash()
    {
        return (Time.time - lastDashTime) >= dashCooldown;
    }


    public bool TryGetHoverGroundSphere(out RaycastHit hit)
    {
        float radius = controller.radius * hoverSphereRadiusMultiplier;

        Vector3 origin = transform.position + Vector3.up * (radius + 0.1f);

        return Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out hit,
            hoverRayLength,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.2f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + Vector3.down * hoverRayLength);
    }
}
