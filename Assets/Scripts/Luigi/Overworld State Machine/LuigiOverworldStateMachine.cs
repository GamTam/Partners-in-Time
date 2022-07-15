using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LuigiOverworldStateMachine : Billboard
{
    // Input
    private PlayerInput _playerInput;
    private InputAction _action;
    private InputAction _switchAction;
    private InputAction _jump;
    private InputAction _moveVector;

    // Stats
    [SerializeField] private int moveSpeed = 5;
    private ArrayList _actions;
    private int _currentAction = 0;

    // State Machine
    private LuigiOverworldBaseState _currentState;
    private LuigiOverworldStateFactory _states;
    
    // Mario
    [SerializeField] public Transform _marioPos;
    public Queue<Vector3> _posQueue;
    private Queue<Quaternion> _rotQueue;
    [SerializeField] private int _queueDelay = 5;

    // Jump
    private float _velocity;
    private float _gravity;
    private float _initialJumpVelocity;
    private float _fallMultiplier = 2f;
    private float _maxJumpHeight = 4f;
    private float _maxJumpTime = 0.75f;

    // Misc.
    private CharacterController _controller;
    [SerializeField] private GameObject child;
    [SerializeField] private TMP_Text _debugData;
    [SerializeField] private Transform _shadow;
    private RaycastHit _hit;
    
    // Getters and Setters
    public LuigiOverworldBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool Action { get { return _action.triggered; } }
    public bool SwitchAction { get { return _switchAction.triggered; } }
    public int CurrentAction { get { return _currentAction; } set { _currentAction = value; } }
    public ArrayList Actions { get { return _actions; } }
    public bool Jump { get { return _jump.triggered; } }
    public Animator Animator { get { return _animator; } }
    public string Facing { get { return _facing; } }
    public Vector2 MoveVector { get {return _moveVector.ReadValue<Vector2>().normalized; } }
    public float MoveAngle {get {return _moveAngle;} set {_moveAngle = value;} }
    public CharacterController Controller {get {return _controller;}}
    public int MoveSpeed {get {return moveSpeed;}}
    public Transform Cam {get {return _cam;}}
    public float InitialJumpVelocity {get {return _initialJumpVelocity;}}
    public float Gravity {get {return _gravity;}}
    public float FallMultiplier {get {return _fallMultiplier;}}
    public float Velocity {get {return _velocity;} set {_velocity = value;}}
    public Transform MarioPos {get {return _marioPos;}}
    public int QueueDelay {get {return _queueDelay;}}
    public Queue<Vector3> PosQueue {get {return _posQueue;} set {_posQueue = value;}}
    public Queue<Quaternion> RotQueue {get {return _rotQueue;} set {_rotQueue = value;}}
    public Transform Shadow {get {return _shadow;} set {_shadow = value;}}

    private void Awake()
    {
        base.Init(child);

        // Input Setup
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        
        _action = _playerInput.actions["l_action"];
        _switchAction = _playerInput.actions["switch_action"];
        _jump = _playerInput.actions["jump"];
        _moveVector = _playerInput.actions["move"];

        _actions = new ArrayList(new[] {"jump"});
        
        // Jump Setup
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(_maxJumpTime / 2, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / (_maxJumpTime / 2);

        // Misc. Setup
        _controller = GetComponent<CharacterController>();
        _posQueue = new Queue<Vector3>();
        _rotQueue = new Queue<Quaternion>();
        
        // State Machine Setup
        _states = new LuigiOverworldStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }
    
    void Update()
    {
        _currentState.UpdateStates();
        _debugData.SetText("Press <sprite=\"" + _playerInput.currentControlScheme + "\" name=\"" 
                                            + _playerInput.actions["l_action"].GetBindingDisplayString()+ 
                                            "\"> To " + _actions[_currentAction]);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _moveAngle, transform.eulerAngles.z);
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit,
            Mathf.Infinity))
        {
            _shadow.transform.position = new Vector3(_shadow.transform.position.x, _hit.point.y,
                _shadow.transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    protected override void SetAnimation()
    {
        _currentState.AnimateState();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.tag == "Block" && _currentState is LuigiOverworldJumpState) {
            _velocity += _gravity * Time.deltaTime * 3;
            hit.transform.SendMessage("OnBlockHit", "Luigi");
        }
    }
}
