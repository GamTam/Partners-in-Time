using System.Collections.Generic;

public class BLuigiOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk,
        Jump,
        Grounded,
        Falling
    }
    
    private BLuigiOverworldStateMachine _context;
    private Dictionary<States, BLuigiOverworldBaseState> _states = new Dictionary<States, BLuigiOverworldBaseState>();

    public BLuigiOverworldStateFactory(BLuigiOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new BLuigiOverworldIdleState(_context, this);
        _states[States.Walk] = new BLuigiOverworldWalkState(_context, this);
        _states[States.Jump] = new BLuigiOverworldJumpState(_context, this);
        _states[States.Grounded] = new BLuigiOverworldGroundedState(_context, this);
        _states[States.Falling] = new BLuigiOverworldFallingState(_context, this);
    }

    public BLuigiOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public BLuigiOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }

    public BLuigiOverworldBaseState Jump()
    {
        return _states[States.Jump];
    }

    public BLuigiOverworldBaseState Grounded()
    {
        return _states[States.Grounded];
    }

    public BLuigiOverworldBaseState Falling()
    {
        return _states[States.Falling];
    }
}
