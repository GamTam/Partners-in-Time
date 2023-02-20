using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldAttackState : EnemyOverworldBaseState, IEnemyOverworldRootState
{
    private Vector3 _newMove;
    private Vector3 _initPlayerPos;
    private float _floatTimer;

    public EnemyOverworldAttackState(EnemyOverworldStateMachine currentContext, EnemyOverworldStateFactory enemyOverworldStateFactory) 
        : base(currentContext, enemyOverworldStateFactory) {}

    public override void EnterState()
    {
        _ctx.Eam.IsMoving = false;
        _ctx.AiDisabled = true;

        if(!_ctx.MoveOnDetection) {
            _ctx.PlayerDetected = false;
            _ctx.FovDisabled = true;
        }

        if(_ctx.FloatingEnemy) {
            _floatTimer = 30f;
        }

        _newMove = new Vector3(0f, 0f, 0f);
        _ctx.Velocity = _ctx.Gravity;
        _initPlayerPos = _ctx.PlayerRef.transform.position;
    }

    public override void UpdateState()
    {
        if(!_ctx.IsLookedAt) {
            Vector3 moveDirection;

            if(!_ctx.FloatingEnemy) {
                moveDirection = _ctx.Eam.GetMoveVector(_initPlayerPos);
            } else {
                _ctx.transform.LookAt(_ctx.PlayerRef.transform);
                moveDirection = _ctx.transform.TransformDirection(Vector3.forward);
            }

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _ctx.Cam.eulerAngles.y;
            _ctx.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            _newMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _ctx.MoveAngle = targetAngle;

            _newMove = _newMove * _ctx.MoveAttackSpeed * Time.deltaTime;

            if(_ctx.FloatingEnemy) {
                Vector3 maxAngleVector = _ctx.StartingPos + (Vector3.right * _ctx.XLimit) + (Vector3.forward * _ctx.ZLimit);
                Vector3 minAngleVector = _ctx.StartingPos + (Vector3.left * _ctx.XLimit) + (Vector3.back * _ctx.ZLimit);
                            
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

            _ctx.Controller.Move(_newMove);

            if(_ctx.FloatingEnemy) {
                _floatTimer += Time.deltaTime;

                float floatY = _ctx.transform.position.y;
                floatY = _ctx.StartingPos.y + (Mathf.Sin(_floatTimer * _ctx.FloatSpeed) * _ctx.FloatStrength);

                _ctx.Controller.enabled = false;
                _ctx.transform.position = new Vector3(_ctx.transform.position.x, floatY, _ctx.transform.position.z);
                _ctx.Controller.enabled = true;
            }
        }

        if(!_ctx.FloatingEnemy) {
            HandleGravity();
        }

        CheckSwitchStates();
    }

    public override void ExitState() {
        _ctx.AiDisabled = false;
        _ctx.FovDisabled = false;
        _ctx.Eam.IsMoving = false;
    }

    public override void CheckSwitchStates()
    {
        if(!_ctx.IsLookedAt) {
            if(!_ctx.MoveOnDetection) {
                if(!_ctx.Eam.IsMoving || _ctx.IsOverLimit(_ctx.transform.position)) {
                    SwitchState(_factory.Idle());
                }
            } else if(!_ctx.PlayerDetected) {
                SwitchState(_factory.Idle());
            }
        } else {
            SwitchState(_factory.Idle());
        }
    }

    public override void AnimateState()
    {
        _ctx.CAnimator.Play(_ctx.AnimPrefix + "_walk" + _ctx.Facing);
    }

    public void HandleGravity() {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}