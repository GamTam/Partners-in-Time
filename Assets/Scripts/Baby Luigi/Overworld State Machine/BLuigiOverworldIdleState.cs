public class BLuigiOverworldIdleState : BLuigiOverworldBaseState
{
    public BLuigiOverworldIdleState(BLuigiOverworldStateMachine currentContext, BLuigiOverworldStateFactory BLuigiOverworldStateFactory) 
        : base(currentContext, BLuigiOverworldStateFactory) {}

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
        _ctx.CAnimator.Play("bl_stand" + _ctx.Facing);
    }
}