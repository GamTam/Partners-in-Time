using System.Collections.Generic;

public class LuigiOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk,
        Jump,
        Grounded,
        Falling,
        Landing,
        SpinAndJump
    }
    
    private LuigiOverworldStateMachine _context;
    private Dictionary<States, LuigiOverworldBaseState> _states = new Dictionary<States, LuigiOverworldBaseState>();

    public LuigiOverworldStateFactory(LuigiOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new LuigiOverworldIdleState(_context, this);
        _states[States.Walk] = new LuigiOverworldWalkState(_context, this);
        _states[States.Jump] = new LuigiOverworldJumpState(_context, this);
        _states[States.Grounded] = new LuigiOverworldGroundedState(_context, this);
        _states[States.Falling] = new LuigiOverworldFallingState(_context, this);
        _states[States.Landing] = new LuigiOverworldLandingState(_context, this);
        _states[States.SpinAndJump] = new LuigiOverworldSpinAndJumpState(_context, this);
    }

    public LuigiOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public LuigiOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }

    public LuigiOverworldBaseState Jump()
    {
        return _states[States.Jump];
    }

    public LuigiOverworldBaseState Grounded()
    {
        return _states[States.Grounded];
    }

    public LuigiOverworldBaseState Falling()
    {
        return _states[States.Falling];
    }
    
    public LuigiOverworldBaseState Landing()
    {
        return _states[States.Landing];
    }

    public LuigiOverworldBaseState SpinAndJump()
    {
        return _states[States.SpinAndJump];
    }
}
