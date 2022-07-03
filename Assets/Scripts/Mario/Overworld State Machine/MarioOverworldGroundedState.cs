using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioOverworldGroundedState : MarioOverworldBaseState
{
    public MarioOverworldGroundedState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {}

    public override void CheckSwitchStates()
    {
        if (_ctx.Jump)
        {
            SwitchState(_factory.Jump());
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
        _currentSubState.AnimateState();
    }
}
