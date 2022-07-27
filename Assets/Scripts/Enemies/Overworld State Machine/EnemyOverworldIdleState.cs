using UnityEngine;

public class EnemyOverworldIdleState : EnemyOverworldBaseState, IEnemyOverworldRootState
{
    public EnemyOverworldIdleState(EnemyOverworldStateMachine currentContext, EnemyOverworldStateFactory enemyOverworldStateFactory) 
        : base(currentContext, enemyOverworldStateFactory) {}

    public override void EnterState()
    {
        _ctx.Velocity = _ctx.Gravity;
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if(_ctx.PlayerDetected) {

            SwitchState(_factory.Jump());
        } else if (_ctx.MoveVector.magnitude > Globals.deadZone)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void AnimateState()
    {
        _ctx.Animator.Play(_ctx.AnimPrefix + "_stand" + _ctx.Facing);
    }

    public void HandleGravity() {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}