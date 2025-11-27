public interface IState
{
    void Enter();
    void Exit();
    void HandleInput();
    void LogicUpdate();
    void PhysicsUpdate();
}
