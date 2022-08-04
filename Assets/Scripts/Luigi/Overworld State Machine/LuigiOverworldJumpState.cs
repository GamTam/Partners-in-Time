using UnityEngine;

public class LuigiOverworldJumpState : LuigiOverworldBaseState, ILuigiOverworldRootState
{
    public LuigiOverworldJumpState(LuigiOverworldStateMachine currentContext, LuigiOverworldStateFactory LuigiOverworldStateFactory) 
        : base(currentContext, LuigiOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _ctx.Velocity = _ctx.InitialJumpVelocity;
        _isRootState = true;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }
    
    public override void FixedUpdate()
    {
        // Do Not
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.Velocity <= 0f)
        {
            SwitchState(_factory.Falling());
        }
        else if (_ctx.Controller.isGrounded)
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
       _ctx.Animator.Play("l_jump" + _ctx.Facing);
    }

    public void HandleGravity()
    {
        float prevVel = _ctx.Velocity;
        _ctx.Velocity = _ctx.Velocity + _ctx.Gravity * Time.deltaTime;
        float avgVel = (prevVel + _ctx.Velocity) / 2;
        _ctx.Controller.Move(new Vector3(0f, avgVel * Time.deltaTime));
    }
}