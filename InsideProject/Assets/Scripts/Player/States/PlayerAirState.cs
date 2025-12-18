using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerController player, StateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        bool grounded = false;

        if (player.DashPressed && player.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
            return;
        }

        if (player.useHover)
        {
            if (player.TryGetHoverGroundSphere(out RaycastHit hit))
            {
                float targetY = hit.point.y + player.hoverHeight;
                float dist = Mathf.Abs(player.transform.position.y - targetY);

                grounded = dist <= player.hoverGroundTolerance && player.velocity.y <= 0f;
            }
        }

        else
        {
            grounded = player.controller.isGrounded && player.velocity.y <= 0f;
        }

        if (grounded)
        {
            player.IsGrounded = true;
            player.lastGroundedTime = Time.time;
            if (player.inputDirection.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(player.MoveState);
            else
                stateMachine.ChangeState(player.IdleState);

            return;
        }
    }

    private void ApplyBetterAirGravity()
    {
        float gravityMultiplier = player.velocity.y < 0f
            ? player.fallGravityMultiplier
            : 1f;

        player.velocity = new Vector3(
            player.velocity.x,
            player.velocity.y + player.gravity * gravityMultiplier * Time.fixedDeltaTime,
            player.velocity.z
        );
    }


    public override void PhysicsUpdate()
    {
        float targetSpeed = player.IsRunning ? player.runSpeed : player.walkSpeed;
        Vector3 horizontalVelocity = player.inputDirection * targetSpeed;

        player.velocity = new Vector3(
            horizontalVelocity.x,
            player.velocity.y,
            horizontalVelocity.z
        );

        ApplyBetterAirGravity();
        MoveFull();
    }
}
