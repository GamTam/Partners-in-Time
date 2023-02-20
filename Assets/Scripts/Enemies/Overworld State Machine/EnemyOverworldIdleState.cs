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
            Vector3 destination = new Vector3(_ctx.transform.position.x, _ctx.StartingPos.y, _ctx.transform.position.z);

            if(_ctx.transform.position != destination) {
                _ctx.Controller.enabled = false;
                _ctx.transform.position = Vector3.Lerp(_ctx.transform.position, destination, _ctx.FloatSpeed / 2);
                _ctx.Controller.enabled = true;
            }
        }

        if(!_ctx.FloatingEnemy) {
            HandleGravity();
        }

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
            } else if (_ctx.MoveVector.magnitude > Globals.DeadZone && !_ctx.MoveOnDetection)
            {
                SwitchState(_factory.Walk());
            }
        }
    }

    public override void AnimateState()
    {
        _ctx.CAnimator.Play(_ctx.AnimPrefix + "_stand" + _ctx.Facing);
    }

    public void HandleGravity() {
        _ctx.Controller.Move(new Vector3(0f, _ctx.Velocity * Time.deltaTime));
    }
}