using UnityEngine;

public class LuigiOverworldLandingState : LuigiOverworldBaseState, ILuigiOverworldRootState
{
    public LuigiOverworldLandingState(LuigiOverworldStateMachine currentContext, LuigiOverworldStateFactory LuigiOverworldStateFactory) 
        : base(currentContext, LuigiOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _ctx.Animator.Play("l_land" + _ctx.Facing);
        _ctx.Velocity = _ctx.Gravity;
        _isRootState = true;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {}
    
    public override void FixedUpdate()
    {
        // Do Not
    }

    public override void CheckSwitchStates()
    {
        
        if (_ctx.MoveVector.magnitude > Globals.deadZone || 
            _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            SwitchState(_factory.Grounded());
        } else if (_ctx.Jump)
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
        // Don't
    }

    public void HandleGravity()
    {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}
