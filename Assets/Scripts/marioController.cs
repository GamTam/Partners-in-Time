using UnityEngine;
using UnityEngine.InputSystem;

public class marioController : MonoBehaviour
{
    // Input
    private PlayerInput playerInput;
    private InputAction action;
    private InputAction moveVector;

    // Stats
    [SerializeField] private int moveSpeed = 5;
    
    // Sprites
    private float moveAngle;
    private float prevMoveAngle;
    private string facing = "_down";
    private Transform cam;
    
    // Animation States
    private const string MARIO_STAND_BABY = "m_bm_stand";
    private const string MARIO_WALK_BABY  = "m_bm_walk";

    // Misc.
    private Rigidbody RB;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        // Input Setup
        playerInput = GetComponent<PlayerInput>();
        
        action = playerInput.actions["m_action"];
        moveVector = playerInput.actions["move"];

        // Misc. Setup
        RB = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 forceMove = new Vector3(moveVector.ReadValue<Vector2>().x, 0, moveVector.ReadValue<Vector2>().y);
        RB.velocity = forceMove * moveSpeed;
        
        moveAngle = Mathf.Rad2Deg * Mathf.Atan2(forceMove.x, forceMove.z) + 180;
        if (forceMove == Vector3.zero)
        {
            moveAngle = 0;
        }
        else
        {
            prevMoveAngle = moveAngle;
        }

        Debug.Log("Cam: " + cam.eulerAngles.y + " Mar: " + transform.eulerAngles.y);
        facing = SetFacing(cam.eulerAngles.y - transform.eulerAngles.y);
        
        SetAnimation();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, prevMoveAngle, transform.eulerAngles.z);
    }

    private void SetAnimation()
    {
        if (moveAngle != 0)
        {
            animator.Play(MARIO_WALK_BABY + facing);
        }
        else
        {
            animator.Play(MARIO_STAND_BABY + facing);
        }
    }
    
    private string SetFacing(float moveAngle)
    {
        while (moveAngle < 0)
        {
            moveAngle += 360;
        }
        
        if (moveAngle < 22.5 || moveAngle > 337.5)  return "_down";
        if (moveAngle < 67.5)   return "_downRight";
        if (moveAngle < 112.5)  return "_right";
        if (moveAngle < 157.5)  return "_upRight";
        if (moveAngle < 202.5)  return "_up";
        if (moveAngle < 247.5)  return "_upLeft";
        if (moveAngle < 292.5)  return "_left";
        if (moveAngle <= 337.5) return "_downLeft";

        return null;
    }
}
