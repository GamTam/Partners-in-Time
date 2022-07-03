using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioOverworldIdleState : MarioOverworldBaseState
{
    public MarioOverworldIdleState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if (_ctx.MoveVector.magnitude > Globals.deadZone)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void InitializeSubState() {}

    public override void AnimateState()
    {
        _ctx.Animator.Play("m_stand" + _ctx.Facing);
    }
}