using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController player, StateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        if (!player.useHover)
            player.IsGrounded = player.controller.isGrounded;

        if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.AirState);
            return;
        }

        if (player.inputDirection.sqrMagnitude <= 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
 
        if (player.JumpPressed)
        {
            Debug.Log("Jump");
            stateMachine.ChangeState(player.JumpState);
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

        base.PhysicsUpdate();
    }
}
