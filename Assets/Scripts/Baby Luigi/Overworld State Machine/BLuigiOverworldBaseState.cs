public abstract class BLuigiOverworldBaseState
{
    protected bool _isRootState = false;
    
    protected BLuigiOverworldStateMachine _ctx;
    protected BLuigiOverworldStateFactory _factory;
    protected BLuigiOverworldBaseState _currentSuperState;
    protected BLuigiOverworldBaseState _currentSubState;

    public BLuigiOverworldBaseState(BLuigiOverworldStateMachine currentContext,
        BLuigiOverworldStateFactory BLuigiOverworldStateFactory)
    {
        _ctx = currentContext;
        _factory = BLuigiOverworldStateFactory;
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

    protected void SwitchState(BLuigiOverworldBaseState newState)
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

    protected void SetSuperState(BLuigiOverworldBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(BLuigiOverworldBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        
    }
}
