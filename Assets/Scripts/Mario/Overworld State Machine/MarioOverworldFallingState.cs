using UnityEngine;

public class MarioOverworldFallingState : MarioOverworldBaseState, IMarioOverworldRootState
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
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.Controller.isGrounded) return;
        SwitchState(_ctx.MoveVector.magnitude > Globals.DeadZone ? _factory.Grounded() : _factory.Landing());
    }

    public override void InitializeSubState()
    {
        if (_ctx.MoveVector.magnitude < Globals.DeadZone)
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
        _ctx.CAnimator.Play("m_fall" + _ctx.Facing);
    }

    public void HandleGravity()
    {
        float prevVel = _ctx.Velocity;
        _ctx.Velocity = _ctx.Velocity + _ctx.Gravity * Time.deltaTime * _ctx.FallMultiplier;
        float avgVel = (prevVel + _ctx.Velocity) / 2;
        _ctx.Controller.Move(new Vector3(0f, avgVel * Time.deltaTime));
    }
}