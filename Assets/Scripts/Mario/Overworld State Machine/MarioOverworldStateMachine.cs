using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Dynamic;
using Cinemachine;

public class MarioOverworldStateMachine : Billboard
{
    // Input
    private PlayerInput _playerInput;
    private InputAction _mAction;
    private InputAction _bmAction;
    private InputAction _blAction;
    private InputAction _switchAction;
    private InputAction _jump;
    private InputAction _moveVector;
    private bool _inputDisabled = false;

    // Stats
    [SerializeField] private int moveSpeed = 5;
    private ArrayList _actions;
    private int _currentAction = 0;

    //State Machine
    private MarioOverworldBaseState _currentState;
    private MarioOverworldStateFactory _states;

    // Jump
    private float _velocity;
    private float _gravity;
    private float _initialJumpVelocity;
    private float _fallMultiplier = 2f;
    private float _maxJumpHeight = 4f;
    private float _maxJumpTime = 0.75f;

    // Misc.
    private CharacterController _controller;
    private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private GameObject child;
    [SerializeField] private TMP_Text _debugData;
    [SerializeField] private Transform _shadow;
    private RaycastHit _hit;
    private Vector3 _lastPosition;
    private bool _fovDisabled = false;
    private Transform _wallTransform;

    // Babies
    [SerializeField] private GameObject _babyMarioRef;

    // Luigi
    [SerializeField] private Transform _luigiPos;
    private LuigiOverworldStateMachine _luigiSM;
    private float _maxDistance = 1.8f;
    private float _collisionDot;
    private bool _angleColliding;
    
    // Getters and Setters
    public MarioOverworldBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool MAction { get { return _mAction.triggered; } }
    public bool SwitchAction { get { return _switchAction.triggered; } }
    public int CurrentAction { get { return _currentAction; } set { _currentAction = value; } }
    public ArrayList Actions { get { return _actions; } }
    public bool Jump { get { return _jump.triggered; } }
    public Animator Animator { get { return _animator; } }
    public string Facing { get { return _facing; } }
    public Vector2 MoveVector { get {return !_inputDisabled ? _moveVector.ReadValue<Vector2>().normalized : Vector3.zero; } }
    public float MoveAngle {get {return _moveAngle;} set {_moveAngle = value;} }
    public CharacterController Controller {get {return _controller;}}
    public int MoveSpeed {get {return moveSpeed;}}
    public Transform Cam {get {return _cam;}}
    public float InitialJumpVelocity {get {return _initialJumpVelocity;}}
    public float Gravity {get {return _gravity;}}
    public float FallMultiplier {get {return _fallMultiplier;}}
    public float Velocity {get {return _velocity;} set {_velocity = value;}}
    public Transform LuigiPos {get {return _luigiPos;}}
    public float MaxDistance {get {return _maxDistance;}}
    public float CollisionDot {get {return _collisionDot;}}
    public bool LuigiAngleColliding {get {return _angleColliding;}}
    public bool InputDisabled { get { return _inputDisabled; } set { _inputDisabled = value; } }
    public bool FovDisabled { get { return _fovDisabled; } set { _fovDisabled = value; } }
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        base.Init(child);

        _lastPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        _luigiSM = _luigiPos.GetComponent<LuigiOverworldStateMachine>();

        // Input Setup
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        _playerInput.SwitchCurrentActionMap("Overworld");
        
        _mAction = _playerInput.actions["m_action"];
        _bmAction = _playerInput.actions["bm_action"];
        _blAction = _playerInput.actions["bl_action"];
        _switchAction = _playerInput.actions["switch_action"];
        _jump = _playerInput.actions["jump"];
        _moveVector = _playerInput.actions["move"];

        _actions = new ArrayList(new[] {"jump", "spin and jump"});
        
        // Jump Setup
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(_maxJumpTime / 2, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / (_maxJumpTime / 2);

        // Misc. Setup
        _controller = GetComponent<CharacterController>();
        _virtualCam = GameObject.FindWithTag("2D Cam").GetComponent<CinemachineVirtualCamera>();

        
        // State Machine Setup
        _states = new MarioOverworldStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }
    
    void Update()
    {   
        if(_wallTransform) {
            Debug.Log(Vector3.Dot(transform.TransformDirection(Vector3.forward), _wallTransform.TransformDirection(Vector3.forward)));
        }
        if(_bmAction.triggered || _blAction.triggered) {
            BMarioOverworldStateMachine babyMarioSM = _babyMarioRef.GetComponent<BMarioOverworldStateMachine>();

            babyMarioSM.InputDisabled = false;
            _inputDisabled = true;
            _virtualCam.Follow = _babyMarioRef.transform;
        }

        if(_inputDisabled) {
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _luigiSM.Sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        } else {
            _sprite.color = new Color(1f, 1f, 1f, 1f);
            _luigiSM.Sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        if(_lastPosition == new Vector3(transform.position.x, 0f, transform.position.z) ||
        (Vector3.Distance(transform.position, _luigiPos.position) < (_maxDistance / 2) && IsHittingWall() &&
        Mathf.Abs(Vector3.Dot(transform.TransformDirection(Vector3.forward), _wallTransform.TransformDirection(Vector3.forward))) > 0.7f)) {
            _luigiSM.StopMovement = true;
        } else {
            _lastPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            _luigiSM.StopMovement = false;
        }

        if(!_fovDisabled) {
            FieldOfViewCheck();
        }

        _currentState.UpdateStates();
        _debugData.SetText("Press <sprite=\"" + _playerInput.currentControlScheme + "\" name=\"" 
                           + _playerInput.actions["m_action"].GetBindingDisplayString() + 
                           "\"> To " + _actions[_currentAction]);
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

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.tag == "Block" && _currentState is MarioOverworldJumpState) {
            _velocity = 0;
            hit.gameObject.SendMessage("OnBlockHit", "Mario");
        }
    }

    public void OnCollision(object[] args) {
        _collisionDot = (float) args[0];
        _angleColliding = (bool) args[1];
    }

    private void FieldOfViewCheck() {
        
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, 5f);

        if(rangeChecks.Length != 0) {
            for(int i = 0; i < rangeChecks.Length; i++) {
                Transform target = rangeChecks[i].transform;

                if(target.gameObject.tag == "Enemy" && target.gameObject.GetComponent<EnemyOverworldStateMachine>().Shy) {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if(Vector3.Angle(transform.forward, directionToTarget) < (160f / 2)) {
                        target.gameObject.SendMessage("OnBooSpotted", true);
                    } else {
                        target.gameObject.SendMessage("OnBooSpotted", false);
                    }
                }
            }
        }
    }

    private bool IsHittingWall() {
        bool IsHitting = false;
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.back, out hit,
         _controller.radius + _controller.skinWidth)) {
            if(hit.transform.gameObject.tag == "Wall") {
                IsHitting = true;
                _wallTransform = hit.transform;
            }
        }

        if(Physics.Raycast(transform.position, Vector3.forward, out hit,
         _controller.radius + _controller.skinWidth)) {
            if(hit.transform.gameObject.tag == "Wall") {
                IsHitting = true;
                _wallTransform = hit.transform;
            }
        }

        if(Physics.Raycast(transform.position, Vector3.left, out hit,
         _controller.radius + _controller.skinWidth)) {
            if(hit.transform.gameObject.tag == "Wall") {
                IsHitting = true;
                _wallTransform = hit.transform;
            }
        }

        if(Physics.Raycast(transform.position, Vector3.right, out hit,
         _controller.radius + _controller.skinWidth)) {
            if(hit.transform.gameObject.tag == "Wall") {
                IsHitting = true;
                _wallTransform = hit.transform;
            }
        }

        return IsHitting;
    }
}