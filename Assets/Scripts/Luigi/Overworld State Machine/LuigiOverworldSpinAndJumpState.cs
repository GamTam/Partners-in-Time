using UnityEngine;

public class LuigiOverworldSpinAndJumpState : LuigiOverworldBaseState, ILuigiOverworldRootState
{
    public LuigiOverworldSpinAndJumpState(LuigiOverworldStateMachine currentContext, LuigiOverworldStateFactory LuigiOverworldStateFactory) 
        : base(currentContext, LuigiOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _ctx.CAnimator.Play("l_spin_and_jump");
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
        
        if (_ctx.CAnimator.NormalizedTime >= 1)
        {
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        // Don't
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
