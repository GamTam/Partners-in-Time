using System.Collections.Generic;

public class BMarioOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk,
        Jump,
        Grounded,
        Falling
    }
    
    private BMarioOverworldStateMachine _context;
    private Dictionary<States, BMarioOverworldBaseState> _states = new Dictionary<States, BMarioOverworldBaseState>();

    public BMarioOverworldStateFactory(BMarioOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new BMarioOverworldIdleState(_context, this);
        _states[States.Walk] = new BMarioOverworldWalkState(_context, this);
        _states[States.Jump] = new BMarioOverworldJumpState(_context, this);
        _states[States.Grounded] = new BMarioOverworldGroundedState(_context, this);
        _states[States.Falling] = new BMarioOverworldFallingState(_context, this);
    }

    public BMarioOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public BMarioOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }

    public BMarioOverworldBaseState Jump()
    {
        return _states[States.Jump];
    }

    public BMarioOverworldBaseState Grounded()
    {
        return _states[States.Grounded];
    }

    public BMarioOverworldBaseState Falling()
    {
        return _states[States.Falling];
    }
}
