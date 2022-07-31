public abstract class BMarioOverworldBaseState
{
    protected bool _isRootState = false;
    
    protected BMarioOverworldStateMachine _ctx;
    protected BMarioOverworldStateFactory _factory;
    protected BMarioOverworldBaseState _currentSuperState;
    protected BMarioOverworldBaseState _currentSubState;

    public BMarioOverworldBaseState(BMarioOverworldStateMachine currentContext,
        BMarioOverworldStateFactory bmarioOverworldStateFactory)
    {
        _ctx = currentContext;
        _factory = bmarioOverworldStateFactory;
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public abstract void AnimateState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(BMarioOverworldBaseState newState)
    {
        ExitState();
        
        newState.EnterState();

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(BMarioOverworldBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(BMarioOverworldBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        
    }
}
