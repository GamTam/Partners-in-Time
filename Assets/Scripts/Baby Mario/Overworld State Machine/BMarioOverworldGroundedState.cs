using UnityEngine;
using UnityEngine.XR;

public class BMarioOverworldGroundedState : BMarioOverworldBaseState, IBMarioOverworldRootState
{
    public BMarioOverworldGroundedState(BMarioOverworldStateMachine currentContext, BMarioOverworldStateFactory bmarioOverworldStateFactory) 
        : base(currentContext, bmarioOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _ctx.Velocity = _ctx.Gravity;
        _isRootState = true;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        if (_ctx.SwitchAction)
        {
            _ctx.CurrentAction += 1;
            if (_ctx.CurrentAction > _ctx.Actions.Count - 1)
            {
                _ctx.CurrentAction = 0;
            }
            Debug.Log(_ctx.Actions[_ctx.CurrentAction]);
        }
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {}

    public override void CheckSwitchStates()
    {
        InitializeSubState();

        if (_ctx.Jump)
        {
            SwitchState(_factory.Jump());
        }
        else if (_ctx.BMAction)
        {
            switch (_ctx.Actions[_ctx.CurrentAction])
            {
                case "jump":
                    SwitchState(_factory.Jump());
                    break;
            }
        }else if (!_ctx.Controller.isGrounded)
        {
            _ctx.Velocity = 0;
            SwitchState(_factory.Falling());
        }
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
        _currentSubState.AnimateState();
    }

    public void HandleGravity()
    {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}
