using UnityEngine;

public class EnemyOverworldJumpState : EnemyOverworldBaseState, IEnemyOverworldRootState
{
    public EnemyOverworldJumpState(EnemyOverworldStateMachine currentContext, EnemyOverworldStateFactory enemyOverworldStateFactory) 
        : base(currentContext, enemyOverworldStateFactory) {}

    public override void EnterState()
    {
        _ctx.Eam.IsMoving = false;
        _ctx.AiDisabled = true;
        _ctx.FovDisabled = true;
        _ctx.PlayerDetected = false;
        _ctx.Velocity = _ctx.InitialJumpVelocity;
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if (_ctx.Controller.isGrounded) {
            SwitchState(_factory.Attack());
        }
    }

    public override void AnimateState()
    {
        _ctx.Animator.Play(_ctx.AnimPrefix + "_stand" + _ctx.Facing);
    }

    public void HandleGravity() {
        float prevVel = _ctx.Velocity;
        _ctx.Velocity = _ctx.Velocity + _ctx.Gravity * Time.deltaTime;
        float avgVel = (prevVel + _ctx.Velocity) / 2;
        _ctx.Controller.Move(new Vector3(0f, avgVel * Time.deltaTime));
    }
}