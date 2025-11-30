using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController player, StateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Jump State");
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
        if (player.velocity.y <= 0f)
        {
            stateMachine.ChangeState(player.AirState);
            return;
        }
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

        ApplyGravity();
        MoveFull();
    }
}
