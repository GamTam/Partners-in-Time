using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldStateMachine : Billboard
{
    // State Machine
    private EnemyOverworldBaseState _currentState;
    private EnemyOverworldStateFactory _states;

    // Movement
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    private Vector3 _moveVector;
    private EnemyActionManager _eam;
    private Vector3 _target;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _xLimit;
    [SerializeField] private float _zLimit;
    private Vector3 _startingPos;
    private bool _aiDisabled = false;
    [SerializeField] private bool _moveOnDetection;
    [SerializeField] private bool _floatingEnemy;
    [SerializeField] private bool _shy;
    [SerializeField] private float _floatSpeed;
    [SerializeField] private float _floatStrength;
    private bool _isLookedAt = false;

    // Pre-attack Jump
    private float _velocity;
    private float _gravity;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 1f;
    private float _maxJumpTime = 0.25f;

    // Attack
    [Header("Attack")]
    [SerializeField] private bool _stationaryAttack;
    [SerializeField] private float _moveAttackSpeed;

    // FOV
    [Header("FOV")]
    [SerializeField] private float _radius;
    [Range(0, 360)]
    [SerializeField] private float _angle;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private GameObject _playerRef;
    private bool _playerDetected = false;
    private bool _fovDisabled = false;

    // Misc.
    private CharacterController _controller;
    [Header("Misc")]
    [SerializeField] private GameObject _child;
    [SerializeField] private string _animPrefix;
    [SerializeField] private Transform _shadow;
    private Vector3 _initChildPosition;
    private RaycastHit _hit;

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
    public float Radius { get {return _radius;} }
    public float Angle { get {return _angle;} }
    public bool PlayerDetected { get {return _playerDetected;} set { _playerDetected = value; } }
    public GameObject PlayerRef { get {return _playerRef;} }
    public bool StationaryAttack { get {return _stationaryAttack;} }
    public float XLimit { get {return _xLimit;} }
    public float ZLimit { get {return _zLimit;} }
    public bool AiDisabled {get {return _aiDisabled;} set {_aiDisabled = value;} }
    public bool FovDisabled {get {return _fovDisabled;} set {_fovDisabled = value;} }
    public float MoveAttackSpeed {get {return _moveAttackSpeed;} }
    public float Velocity { get {return _velocity;} set {_velocity = value;}}
    public float Gravity { get {return _gravity;}}
    public float InitialJumpVelocity { get {return _initialJumpVelocity;}}
    public bool MoveOnDetection { get {return _moveOnDetection;}}
    public bool FloatingEnemy { get {return _floatingEnemy;}}
    public Vector3 StartingPos { get {return _startingPos;}}
    public bool Shy { get {return _shy;}}
    public bool IsLookedAt { get {return _isLookedAt;}}
    public GameObject Child { get {return _child;}}
    public Vector3 InitChildPosition { get {return _initChildPosition;}}
    public float FloatSpeed { get {return _floatSpeed;}}
    public float FloatStrength { get {return _floatStrength;}}
 
    private void Awake() {
        Init(_child);

        _startingPos = transform.position;
        _initChildPosition = _child.transform.position;

        // FOV
        StartCoroutine(ViewOfField());

        // Jump Setup
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(_maxJumpTime / 2, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / (_maxJumpTime / 2);

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

        //_isLookedAt = _playerRef.transform.GetComponent<MarioOverworldStateMachine>().BooSpotted;

        _currentState.UpdateState();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _moveAngle, transform.eulerAngles.z);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit,
            Mathf.Infinity))
        {
            _shadow.transform.position = new Vector3(_shadow.transform.position.x, _hit.point.y,
                _shadow.transform.position.z);
        }
    }

    protected override void SetAnimation()
    {
        _currentState.AnimateState();
    }

    private IEnumerator EnemyAI() {
        while(true) {

            if(!_aiDisabled && !_moveOnDetection) {
                yield return new WaitForSeconds(_eam.IsMoving ? Time.deltaTime : _idleTime);
            } else {
                yield return new WaitForSeconds(Time.deltaTime);
            }


            if(!_aiDisabled && !_moveOnDetection) {
                if(!_eam.IsMoving) {
                    
                    do {
                        int direction  = Random.Range(1, 5);
                        float distance = Random.Range(2f, 5f);

                        _target = transform.position;

                        switch(direction) {
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
                    } while(IsOverLimit(_target));
                }

                _moveVector = _eam.GetMoveVector(_target);
            }
        }
    }

    public bool IsOverLimit(Vector3 target) {
        return (target.x > (_startingPos.x + _xLimit)) || (target.x < (_startingPos.x - _xLimit)) ||
                    (target.z > (_startingPos.z + _zLimit)) || (target.z < (_startingPos.z - _zLimit));
    }
    

    private IEnumerator ViewOfField() {
        while(true) {
            yield return new WaitForSeconds(0.1f);

            if(!_fovDisabled) {
                FieldOfViewCheck();
            }
        }
    }

    private void FieldOfViewCheck() {
        
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < (_angle / 2)) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget)) {
                    _playerDetected = true;
                } else {
                    _playerDetected = false;
                }
            } else {
                _playerDetected = false;
            }
        } else if(_playerDetected) {
            _playerDetected = false;
        }
    }

    public void OnBooSpotted(bool isSpotted) {
        _isLookedAt = isSpotted;
        Debug.Log(isSpotted);
    }
}