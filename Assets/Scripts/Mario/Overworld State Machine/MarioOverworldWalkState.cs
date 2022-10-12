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
        
        if (_ctx.MoveVector.magnitude > Globals.DeadZone) {
            _ctx.MoveAngle = targetAngle;

            Vector3 tempMarioPos = _ctx.transform.position;
            Vector3 tempLuigiPos = _ctx.LuigiPos.position;

            tempMarioPos.y = 0f;
            tempLuigiPos.y = 0f;

            float distance = Vector3.Distance(tempMarioPos, tempLuigiPos);

            _newMove = _newMove * _ctx.MoveSpeed * Time.deltaTime;
            if(distance >= _ctx.MaxDistance) {
                Vector3 moveN = _newMove.normalized;
                float dotDirection = Vector3.Dot(_ctx.transform.TransformDirection(Vector3.forward).normalized, _ctx.LuigiPos.TransformDirection(Vector3.forward).normalized);

                
                if(!_ctx.LuigiAngleColliding) {
                    if(dotDirection >= 0f) {
                        if(moveN.z != 0f && Mathf.Abs(_ctx.CollisionDot) >= 0.9f) {
                            _newMove.z = 0f;
                        }

                        if(moveN.x != 0f && Mathf.Abs(_ctx.CollisionDot) < 0.5f) {
                            _newMove.x = 0f;
                        }
                    }
                } else {
                    Vector3 maxAngleVector = _ctx.LuigiPos.position + (Vector3.right * _ctx.MaxDistance) + (Vector3.forward * _ctx.MaxDistance);
                    Vector3 minAngleVector = _ctx.LuigiPos.position + (Vector3.left * _ctx.MaxDistance) + (Vector3.back * _ctx.MaxDistance);
                        
                    if(_ctx.transform.position.x >= maxAngleVector.x && _newMove.x > 0) {
                        _newMove.x = 0f;
                    }
                        
                    if(_ctx.transform.position.z >= maxAngleVector.z && _newMove.z > 0) {
                        _newMove.z = 0f;
                    }

                    if(_ctx.transform.position.x <= minAngleVector.x && _newMove.x < 0) {
                        _newMove.x = 0f;
                    }

                    if(_ctx.transform.position.z <= minAngleVector.z && _newMove.z < 0) {
                        _newMove.z = 0f;
                    }
                }
            }
            //if(!_ctx.InputDisabled) {
                _ctx.Controller.Move(_newMove);
            //} 
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.MoveVector.magnitude < Globals.DeadZone)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    public override void AnimateState()
    {
        
        _ctx.CAnimator.Play("m_walk" + _ctx.Facing);
    }
}
