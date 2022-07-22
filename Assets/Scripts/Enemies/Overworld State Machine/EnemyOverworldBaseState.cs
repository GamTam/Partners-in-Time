public abstract class EnemyOverworldBaseState
{
    protected EnemyOverworldStateMachine _ctx;
    protected EnemyOverworldStateFactory _factory;
    protected EnemyOverworldBaseState _currentState;

    public EnemyOverworldBaseState(EnemyOverworldStateMachine currentContext,
        EnemyOverworldStateFactory enemyOverworldStateFactory)
    {
        _ctx = currentContext;
        _factory = enemyOverworldStateFactory;
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void AnimateState();

    protected void SwitchState(EnemyOverworldBaseState newState)
    {
        ExitState();
        
        newState.EnterState();
        _ctx.CurrentState = newState;
    }

    protected void SetState(EnemyOverworldBaseState newState)
    {
        _currentState = newState;
    }
}
