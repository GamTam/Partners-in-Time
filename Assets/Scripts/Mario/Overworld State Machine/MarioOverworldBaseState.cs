using UnityEngine;

public abstract class MarioOverworldBaseState
{
    protected bool _isRootState = false;
    
    protected MarioOverworldStateMachine _ctx;
    protected MarioOverworldStateFactory _factory;
    protected MarioOverworldBaseState _currentSuperState;
    protected MarioOverworldBaseState _currentSubState;

    public MarioOverworldBaseState(MarioOverworldStateMachine currentContext,
        MarioOverworldStateFactory marioOverworldStateFactory)
    {
        _ctx = currentContext;
        _factory = marioOverworldStateFactory;
    }
    
    public MarioOverworldBaseState CurrentSubState => _currentSubState;
    
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

    protected void SwitchState(MarioOverworldBaseState newState)
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

    protected void SetSuperState(MarioOverworldBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(MarioOverworldBaseState newSubState, bool enterState=false)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        if (enterState) newSubState.EnterState();
    }
}
