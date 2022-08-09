public class BMarioOverworldIdleState : BMarioOverworldBaseState
{
    public BMarioOverworldIdleState(BMarioOverworldStateMachine currentContext, BMarioOverworldStateFactory bmarioOverworldStateFactory) 
        : base(currentContext, bmarioOverworldStateFactory) {}

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
        if (_ctx.MoveVector.magnitude > Globals.DeadZone)
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void InitializeSubState() {}

    public override void AnimateState()
    {
        _ctx.CAnimator.Play("bm_stand" + _ctx.Facing);
    }
}