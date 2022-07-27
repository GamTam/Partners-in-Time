using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldAttackState : EnemyOverworldBaseState, IEnemyOverworldRootState
{
    private Vector3 _newMove;
    private Vector3 _initPlayerPos;

    public EnemyOverworldAttackState(EnemyOverworldStateMachine currentContext, EnemyOverworldStateFactory enemyOverworldStateFactory) 
        : base(currentContext, enemyOverworldStateFactory) {}

    public override void EnterState()
    {
        _newMove = new Vector3(0f, 0f, 0f);
        _ctx.Velocity = _ctx.Gravity;
        _initPlayerPos = _ctx.PlayerRef.transform.position;
    }

    public override void UpdateState()
    {
        Vector3 moveDirection = _ctx.Eam.GetMoveVector(_initPlayerPos);

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _ctx.Cam.eulerAngles.y;
        _ctx.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        _newMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _ctx.MoveAngle = targetAngle;

        _newMove = _newMove * _ctx.MoveAttackSpeed * Time.deltaTime;

        _ctx.Controller.Move(_newMove);

        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState() {
        _ctx.AiDisabled = false;
        _ctx.FovDisabled = false;
        _ctx.Eam.IsMoving = false;
    }

    public override void CheckSwitchStates()
    {
        if(!_ctx.Eam.IsMoving || _ctx.IsOverLimit(_ctx.transform.position)) {
            SwitchState(_factory.Idle());
        }
    }

    public override void AnimateState()
    {
        _ctx.Animator.Play(_ctx.AnimPrefix + "_walk" + _ctx.Facing);
    }

    public void HandleGravity() {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}