using System.Collections.Generic;

public class MarioOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk,
        Jump,
        Grounded,
        Falling,
        Landing
    }
    
    private MarioOverworldStateMachine _context;
    private Dictionary<States, MarioOverworldBaseState> _states = new Dictionary<States, MarioOverworldBaseState>();

    public MarioOverworldStateFactory(MarioOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new MarioOverworldIdleState(_context, this);
        _states[States.Walk] = new MarioOverworldWalkState(_context, this);
        _states[States.Jump] = new MarioOverworldJumpState(_context, this);
        _states[States.Grounded] = new MarioOverworldGroundedState(_context, this);
        _states[States.Falling] = new MarioOverworldFallingState(_context, this);
        _states[States.Landing] = new MarioOverworldLandingState(_context, this);
    }

    public MarioOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public MarioOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }

    public MarioOverworldBaseState Jump()
    {
        return _states[States.Jump];
    }

    public MarioOverworldBaseState Grounded()
    {
        return _states[States.Grounded];
    }

    public MarioOverworldBaseState Falling()
    {
        return _states[States.Falling];
    }
    
    public MarioOverworldBaseState Landing()
    {
        return _states[States.Landing];
    }
}
