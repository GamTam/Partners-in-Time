using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioOverworldStateMachine : Billboard
{
    // Input
    private PlayerInput _playerInput;
    private InputAction _action;
    private InputAction _jump;
    private InputAction _moveVector;

    // Stats
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private float gravSpeed = 5;
    [SerializeField] private int maxGrav = 10;
    [SerializeField] private float activeGrav = 0f;

    //State Machine
    private MarioOverworldBaseState _currentState;
    private MarioOverworldStateFactory _states;

    // Misc.
    private CharacterController _controller;
    [SerializeField] private GameObject child;
    
    // Getters and Setters
    public MarioOverworldBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool Jump { get { return _jump.triggered; } }
    public Animator Animator { get { return _animator; } }
    public string Facing { get { return _facing; } }
    public Vector2 MoveVector { get {return _moveVector.ReadValue<Vector2>().normalized; } }
    public float MoveAngle {get {return _moveAngle;} set {_moveAngle = value;} }
    public CharacterController Controller {get {return _controller;}}
    public int MoveSpeed {get {return moveSpeed;}}
    public Transform Cam {get {return _cam;}}
    
    private void Awake()
    {
        base.Init(child);

        // Input Setup
        _playerInput = GetComponent<PlayerInput>();
        
        _action = _playerInput.actions["m_action"];
        _jump = _playerInput.actions["jump"];
        _moveVector = _playerInput.actions["move"];

        // Misc. Setup
        _controller = GetComponent<CharacterController>();
        
        // State Machine Setup
        _states = new MarioOverworldStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _moveAngle, transform.eulerAngles.z);
    }

    protected override void SetAnimation()
    {
        _currentState.AnimateState();
    }
}
