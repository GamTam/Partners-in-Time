using UnityEngine;

public class MarioOverworldFallingState : MarioOverworldBaseState
{
    public MarioOverworldFallingState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory)
    {
    }

    public override void EnterState()
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        float prevVel = _ctx.Velocity;
        _ctx.Velocity = _ctx.Velocity + _ctx.Gravity * Time.deltaTime * _ctx.FallMultiplier;
        float avgVel = (prevVel + _ctx.Velocity) / 2;
        _ctx.Controller.Move(new Vector3(0f, avgVel * Time.deltaTime));
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.Controller.isGrounded)
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
        _ctx.Animator.Play("m_fall" + _ctx.Facing);
    }
}