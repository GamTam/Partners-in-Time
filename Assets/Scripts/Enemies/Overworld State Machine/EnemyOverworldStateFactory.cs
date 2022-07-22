using System.Collections.Generic;

public class EnemyOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk
    }
    
    private EnemyOverworldStateMachine _context;
    private Dictionary<States, EnemyOverworldBaseState> _states = new Dictionary<States, EnemyOverworldBaseState>();

    public EnemyOverworldStateFactory(EnemyOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new EnemyOverworldIdleState(_context, this);
        _states[States.Walk] = new EnemyOverworldWalkState(_context, this);
    }

    public EnemyOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public EnemyOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }
}
