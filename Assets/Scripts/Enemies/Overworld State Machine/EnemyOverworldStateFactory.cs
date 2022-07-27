using System.Collections.Generic;

public class EnemyOverworldStateFactory
{
    enum States
    {
        Idle,
        Walk,
        Jump,
        Attack
    }
    
    private EnemyOverworldStateMachine _context;
    private Dictionary<States, EnemyOverworldBaseState> _states = new Dictionary<States, EnemyOverworldBaseState>();

    public EnemyOverworldStateFactory(EnemyOverworldStateMachine currentContext)
    {
        _context = currentContext;
        _states[States.Idle] = new EnemyOverworldIdleState(_context, this);
        _states[States.Walk] = new EnemyOverworldWalkState(_context, this);
        _states[States.Jump] = new EnemyOverworldJumpState(_context, this);
        _states[States.Attack] = new EnemyOverworldAttackState(_context, this);
    }

    public EnemyOverworldBaseState Idle()
    {
        return _states[States.Idle];
    }

    public EnemyOverworldBaseState Walk()
    {
        return _states[States.Walk];
    }

    public EnemyOverworldBaseState Jump()
    {
        return _states[States.Jump];
    }

    public EnemyOverworldBaseState Attack()
    {
        return _states[States.Attack];
    }
}
