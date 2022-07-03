public class MarioOverworldStateFactory
{
    private MarioOverworldStateMachine _context;

    public MarioOverworldStateFactory(MarioOverworldStateMachine currentContext)
    {
        _context = currentContext;
    }

    public MarioOverworldBaseState Idle()
    {
        return new MarioOverworldIdleState(_context, this);
    }

    public MarioOverworldBaseState Walk()
    {
        return new MarioOverworldWalkState(_context, this);
    }

    public MarioOverworldBaseState Jump()
    {
        return new MarioOverworldJumpState(_context, this);
    }

    public MarioOverworldBaseState Grounded()
    {
        return new MarioOverworldGroundedState(_context, this);
    }
}
