using UnityEngine;

public class MarioOverworldJumpState : MarioOverworldBaseState
{
    private float _timer;
    public MarioOverworldJumpState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _isRootState = true;
        InitializeSubState();
        _timer = 5f;
    }

    public override void UpdateState()
    {
        _timer -= Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (_timer <= 0)
        {
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        if (_ctx.MoveVector.magnitude < Globals.deadZone)
        {
            SetSubState(_factory.Idle());
        }
        else
        {
            SetSubState(_factory.Walk());
        }
    }

    public override void AnimateState()
    {
        
    }
}