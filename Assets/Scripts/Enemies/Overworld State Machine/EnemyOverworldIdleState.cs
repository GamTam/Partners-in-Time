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
        if(_ctx.FloatingEnemy) {
            Vector3 destination = new Vector3(_ctx.Child.transform.position.x, _ctx.InitChildPosition.y, _ctx.Child.transform.position.z);

            if(_ctx.Child.transform.position != destination) {
                _ctx.Child.transform.position = Vector3.Lerp(_ctx.Child.transform.position, destination, _ctx.FloatSpeed / 2);
            }
        }

        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if(!_ctx.IsLookedAt) {
            if(_ctx.PlayerDetected) {
                if(!_ctx.FloatingEnemy) {
                    SwitchState(_factory.Jump());
                } else {
                    SwitchState(_factory.Attack());
                }
            } else if (_ctx.MoveVector.magnitude > Globals.deadZone && !_ctx.MoveOnDetection)
            {
                SwitchState(_factory.Walk());
            }
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