using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController player, StateMachine stateMachine): base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.velocity = new Vector3(0f, player.velocity.y, 0f);
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

        if (player.DashPressed && player.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
            return;
        }


        if (player.JumpPressed)
        {
            Debug.Log("Jump");
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        if (player.inputDirection.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(player.MoveState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        player.velocity = new Vector3(0f, player.velocity.y, 0f);
        base.PhysicsUpdate();
    }
}
