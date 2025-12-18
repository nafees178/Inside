using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController player, StateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        float jumpVelocity = Mathf.Sqrt(player.jumpHeight * -2f * player.gravity);

        player.velocity = new Vector3(
            player.velocity.x,
            jumpVelocity,
            player.velocity.z
        );

        player.IsGrounded = false;
    }

    public override void LogicUpdate()
    {
        if (player.DashPressed && player.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
            return;
        }

        if (player.velocity.y <= 0f)
        {
            stateMachine.ChangeState(player.AirState);
            return;
        }
    }

    private void ApplyBetterJumpGravity()
    {
        float gravityMultiplier = 1f;

        if (player.velocity.y < 0f)
        {
            gravityMultiplier = player.fallGravityMultiplier;
        }
        else if (player.velocity.y > 0f && !player.JumpPressed)
        {
            gravityMultiplier = player.lowJumpGravityMultiplier;
        }

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

        ApplyBetterJumpGravity();
        MoveFull();
    }

}
