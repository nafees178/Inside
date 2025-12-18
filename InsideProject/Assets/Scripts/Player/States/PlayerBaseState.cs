using UnityEngine;

public abstract class PlayerBaseState : IState
{
    protected readonly PlayerController player;
    protected readonly StateMachine stateMachine;

    protected PlayerBaseState(PlayerController player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate()
    {
        if (player.useHover)
        {
            HandleHoverVertical();
            MoveHorizontal();
        }
        else
        {
            ApplyGravity();
            MoveFull();
        }
    }

    private void HandleHoverVertical()
    {
        if (player.TryGetHoverGroundSphere(out RaycastHit hit))
        {
            float targetY = hit.point.y + player.hoverHeight;
            Vector3 pos = player.transform.position;
            pos.y = Mathf.Lerp(pos.y, targetY, player.hoverLerpSpeed * Time.fixedDeltaTime);
            player.transform.position = pos;

            player.IsGrounded = Mathf.Abs(pos.y - targetY) <= player.hoverGroundTolerance;
            if (player.IsGrounded)
                player.velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);
        }
        else
        {
            player.IsGrounded = false;
            player.velocity = new Vector3(player.velocity.x,
                                          player.velocity.y + player.gravity * Time.fixedDeltaTime,
                                          player.velocity.z);

            player.controller.Move(Vector3.up * player.velocity.y * Time.fixedDeltaTime);
        }
    }

    private void MoveHorizontal()
    {
        Vector3 hVel = new Vector3(player.velocity.x, 0f, player.velocity.z);
        player.controller.Move(hVel * Time.fixedDeltaTime);
    }

    protected void ApplyGravity()
    {
        if (player.IsGrounded && player.velocity.y < 0f)
            player.velocity = new Vector3(player.velocity.x, -2f, player.velocity.z);

        player.velocity = new Vector3(player.velocity.x,
                                      player.velocity.y + player.gravity * Time.fixedDeltaTime,
                                      player.velocity.z);
    }

    protected void MoveFull()
    {
        player.controller.Move(player.velocity * Time.fixedDeltaTime);
    }
}
