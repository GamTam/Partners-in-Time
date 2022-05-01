using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private string dasd;
    
    // Sprites
    [SerializeField] private float moveAngle;
    private float prevMoveAngle;
    private string facing = "_down";
    
    // Animation States
    private const string MARIO_STAND_BABY          = "m_bm_stand";
    private const string MARIO_WALK_BABY           = "m_bm_walk";

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
        
        SetAnimation();
        transform.eulerAngles = new Vector3(transform.rotation.x, prevMoveAngle, transform.rotation.z);
    }

    private void SetAnimation()
    {
        if (moveAngle < 22.5 || moveAngle > 337.5)  dasd = "_down";
        else if (moveAngle < 67.5)   dasd = "_downLeft";
        else if (moveAngle < 112.5)  dasd = "_left";
        else if (moveAngle < 157.5)  dasd = "_upLeft";
        else if (moveAngle < 202.5)  dasd = "_up";
        else if (moveAngle < 247.5)  dasd = "_upRight";
        else if (moveAngle < 292.5)  dasd = "_right";
        else if (moveAngle <= 337.5) dasd = "_downRight";
        
        if (moveAngle != 0)
        {
            animator.Play(MARIO_WALK_BABY + facing);
        }
        else
        {
            animator.Play(MARIO_STAND_BABY + facing);
        }
    }
}
