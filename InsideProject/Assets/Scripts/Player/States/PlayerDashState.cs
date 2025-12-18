using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector3 _dashDirection;
    private float _dashTimer;

    public PlayerDashState(PlayerController player, StateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Dash state");
        _dashTimer = 0f;

        Vector3 dir = player.inputDirection;

        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = player.transform.forward;
        }

        _dashDirection = dir.normalized;

        player.velocity = new Vector3(
            _dashDirection.x * player.dashSpeed,
            player.velocity.y,
            _dashDirection.z * player.dashSpeed
        );

        player.lastDashTime = Time.time;

        if (player.cameraEffects != null)
        {
            player.cameraEffects.OnDash(_dashDirection);
        }
    }

    public override void LogicUpdate()
    {
        _dashTimer += Time.deltaTime;

        if (_dashTimer >= player.dashDuration)
        {
            if (player.IsGrounded)
            {
                if (player.inputDirection.sqrMagnitude > 0.01f)
                    stateMachine.ChangeState(player.MoveState);
                else
                    stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.AirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        float dashHorizSpeed = player.dashSpeed;

        Vector3 horizontalVelocity = _dashDirection * dashHorizSpeed;

        player.velocity = new Vector3(
            horizontalVelocity.x,
            player.velocity.y,
            horizontalVelocity.z
        );

        ApplyGravity();
        MoveFull();
    }
}
