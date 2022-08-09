using UnityEngine;

public class BLuigiOverworldFallingState : BLuigiOverworldBaseState, IBLuigiOverworldRootState
{
    public BLuigiOverworldFallingState(BLuigiOverworldStateMachine currentContext, BLuigiOverworldStateFactory BLuigiOverworldStateFactory) 
        : base(currentContext, BLuigiOverworldStateFactory)
    {
    }

    public override void EnterState()
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void FixedUpdate()
    {
        // Do Not
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
        SwitchState(_factory.Grounded());
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
        _ctx.CAnimator.Play("bl_fall" + _ctx.Facing);
    }

    public void HandleGravity()
    {
        float prevVel = _ctx.Velocity;
        _ctx.Velocity = _ctx.Velocity + _ctx.Gravity * Time.deltaTime * _ctx.FallMultiplier;
        float avgVel = (prevVel + _ctx.Velocity) / 2;
        _ctx.Controller.Move(new Vector3(0f, avgVel * Time.deltaTime));
    }
}