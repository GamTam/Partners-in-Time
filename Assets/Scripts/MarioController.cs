using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController : Billboard
{
    // Input
    private PlayerInput playerInput;
    private InputAction action;
    private InputAction moveVector;

    // Stats
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private float gravSpeed = 5;
    [SerializeField] private int maxGrav = 10;
    [SerializeField] private float activeGrav = 0f;

    // Animation States
    private const string MARIO_STAND = "m_stand";
    private const string MARIO_WALK  = "m_walk";

    // Misc.
    private CharacterController controller;
    [SerializeField] private GameObject child;

    private void Awake()
    {
        base.Init(child);
        
        // Input Setup
        playerInput = GetComponent<PlayerInput>();
        
        action = playerInput.actions["m_action"];
        moveVector = playerInput.actions["move"];

        // Misc. Setup
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 forceMove = new Vector3(moveVector.ReadValue<Vector2>().x, 0, moveVector.ReadValue<Vector2>().y).normalized;
        Vector3 newMove = new Vector3(0f, 0f, 0f);

        if (forceMove.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(forceMove.x, forceMove.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            newMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveAngle = targetAngle;
            
            controller.Move(newMove * moveSpeed * Time.deltaTime);
        }

        if (!controller.isGrounded)
        {
            activeGrav = activeGrav < maxGrav ? activeGrav + gravSpeed * Time.deltaTime : maxGrav;
            controller.Move(new Vector3(0, -activeGrav, 0f));
        }
        else
        {
            activeGrav = 0;
        }

        if (forceMove == Vector3.zero)
        {
            moveAngle = float.MinValue;
        }
        else
        {
            prevMoveAngle = moveAngle;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, prevMoveAngle, transform.eulerAngles.z);
    }
    
    protected override void SetAnimation()
    {
        if (moveAngle != float.MinValue)
        {
            animator.Play(MARIO_WALK + facing);
        }
        else
        {
            animator.Play(MARIO_STAND + facing);
        }
    }
}
