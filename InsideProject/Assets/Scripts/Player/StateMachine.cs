public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        if (CurrentState == newState) return;

        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
