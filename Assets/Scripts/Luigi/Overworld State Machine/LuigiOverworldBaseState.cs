public abstract class LuigiOverworldBaseState
{
    protected bool _isRootState = false;
    
    protected LuigiOverworldStateMachine _ctx;
    protected LuigiOverworldStateFactory _factory;
    protected LuigiOverworldBaseState _currentSuperState;
    protected LuigiOverworldBaseState _currentSubState;

    public LuigiOverworldBaseState(LuigiOverworldStateMachine currentContext,
        LuigiOverworldStateFactory LuigiOverworldStateFactory)
    {
        _ctx = currentContext;
        _factory = LuigiOverworldStateFactory;
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();
    
    public abstract void FixedUpdate();

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

    public void FixedUpdateStates()
    {
        FixedUpdate();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdate();
        }
    }

    protected void SwitchState(LuigiOverworldBaseState newState)
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

    protected void SetSuperState(LuigiOverworldBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(LuigiOverworldBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        
    }
}
