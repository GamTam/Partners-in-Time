using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioOverworldWalkState : MarioOverworldBaseState
{
    private Vector3 _newMove;
    
    public MarioOverworldWalkState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}
    
    public override void EnterState()
    {
        _newMove = new Vector3(0f, 0f, 0f);
    }

    public override void UpdateState()
    {
        float targetAngle = Mathf.Atan2(_ctx.MoveVector.x, _ctx.MoveVector.y) * Mathf.Rad2Deg + _ctx.Cam.eulerAngles.y;
        _ctx.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        _newMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        if (_ctx.MoveVector.magnitude > Globals.deadZone) {
            _ctx.MoveAngle = targetAngle;
        }

        _ctx.Controller.Move(_newMove * _ctx.MoveSpeed * Time.deltaTime);
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.MoveVector.magnitude < Globals.deadZone)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    public override void AnimateState()
    {
        _ctx.Animator.Play("m_walk" + _ctx.Facing);
    }
}
