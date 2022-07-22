using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldStateMachine : Billboard
{
    // Stats
    [SerializeField] private float _moveSpeed;

    // Movement
    private Vector3 _moveVector;
    private EnemyActionManager _eam;
    private Vector3 _target;

    // Animation
    [SerializeField] private string _animPrefix;

    // State Machine
    private EnemyOverworldBaseState _currentState;
    private EnemyOverworldStateFactory _states;
    [SerializeField] private GameObject _child;

    // Getters and Setters
    public EnemyOverworldBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Vector3 MoveVector { get { return _moveVector; } set { _moveVector = value; } }
    public string Facing { get { return _facing; } set { _facing = value; } }
    public string AnimPrefix { get { return _animPrefix; } set { _animPrefix = value; } }
    public Animator Animator { get { return _animator; } }
    public float MoveAngle {get {return _moveAngle;} set {_moveAngle = value;} }
    public Transform Cam {get {return _cam;} set {_cam = value;} }
    public float MoveSpeed {get {return _moveSpeed;} }
    public CharacterController Controller { get {return _controller;} }
    public EnemyActionManager Eam { get {return _eam;} }

    // Misc.
    private CharacterController _controller;

    private void Awake() {
        Init(_child);

        // Misc. Setup
        _controller = GetComponent<CharacterController>();
        _eam = new EnemyActionManager(gameObject);

        // State Machine Setup
        _states = new EnemyOverworldStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    private void Start() {
        StartCoroutine(EnemyAI());
    }

    private void Update() {
        _currentState.UpdateState();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _moveAngle, transform.eulerAngles.z);
    }

    protected override void SetAnimation()
    {
        _currentState.AnimateState();
    }

    IEnumerator EnemyAI() {
        while (true) {
            yield return new WaitForSeconds(_eam.IsMoving ? Time.deltaTime : 2f);

            if(!_eam.IsMoving) {

                int direction  = Random.Range(1, 5);
                float distance = Random.Range(2f, 5f);

                _target = transform.position;

                Debug.Log(direction + " : " + distance);

                switch (direction) {
                    case 1:
                        // Up
                        _target += new Vector3(0f, 0f, distance);
                        break;
                    case 2:
                        // Right
                        _target += new Vector3(distance, 0f, 0f);
                        break;
                    case 3:
                        // Down
                        _target += new Vector3(0f, 0f, -distance);
                        break;
                    case 4:
                        // Left
                        _target += new Vector3(-distance, 0f, 0f);
                        break;
                }
            }
            
            _moveVector = _eam.GetMoveVector(_target);
        }
    }
}
