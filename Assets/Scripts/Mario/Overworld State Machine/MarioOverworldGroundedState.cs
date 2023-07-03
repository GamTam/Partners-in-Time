using UnityEngine;
using UnityEngine.XR;

public class MarioOverworldGroundedState : MarioOverworldBaseState, IMarioOverworldRootState
{
    public MarioOverworldGroundedState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}
    
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
            if (_ctx.CurrentAction > _ctx.Actions.Count - 3)
            {
                _ctx.CurrentAction = 0;
            }
        }
        HandleGravity();
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
        else if (_ctx.MAction)
        {
            switch (_ctx.Actions[_ctx.CurrentAction])
            {
                case "jump":
                    SwitchState(_factory.Jump());
                    break;
                case "spin and jump":
                    SwitchState(_factory.SpinAndJump());
                    break;
            }
        }
        else if (!_ctx.Controller.isGrounded)
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
        RaycastHit hit;
        if (Physics.Raycast(_ctx.transform.position, _ctx.transform.TransformDirection(Vector3.down), out hit, 0.5f))
        {
            _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
        }
    }
}
