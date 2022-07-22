public class EnemyOverworldIdleState : EnemyOverworldBaseState
{
    public EnemyOverworldIdleState(EnemyOverworldStateMachine currentContext, EnemyOverworldStateFactory enemyOverworldStateFactory) 
        : base(currentContext, enemyOverworldStateFactory) {}

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if (_ctx.MoveVector.magnitude > Globals.deadZone)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void AnimateState()
    {
        _ctx.Animator.Play(_ctx.AnimPrefix + "_stand" + _ctx.Facing);
    }   
}