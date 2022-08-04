public class LuigiOverworldIdleState : LuigiOverworldBaseState
{
    public LuigiOverworldIdleState(LuigiOverworldStateMachine currentContext, LuigiOverworldStateFactory LuigiOverworldStateFactory) 
        : base(currentContext, LuigiOverworldStateFactory) {}

    public override void EnterState()
    {
        
    }
    
    public override void FixedUpdate()
    {
        // Do Not
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

    public override void InitializeSubState() {}

    public override void AnimateState()
    {
        _ctx.CAnimator.Play("l_stand" + _ctx.Facing);
    }
}