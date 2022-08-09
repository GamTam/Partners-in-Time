public class MarioOverworldIdleState : MarioOverworldBaseState
{
    public MarioOverworldIdleState(MarioOverworldStateMachine currentContext, MarioOverworldStateFactory marioOverworldStateFactory) 
        : base(currentContext, marioOverworldStateFactory) {}

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
        _ctx.CAnimator.Play("m_stand" + _ctx.Facing);
    }
}