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
    
    // Sprites
    private float moveAngle;
    private float prevMoveAngle;
    
    // Animation States
    private const string MARIO_STAND_BABY_UP          = "m_bm_stand_up";
    private const string MARIO_STAND_BABY_DOWN        = "m_bm_stand_down";
    private const string MARIO_STAND_BABY_LEFT        = "m_bm_stand_left";
    private const string MARIO_STAND_BABY_RIGHT       = "m_bm_stand_right";
    
    private const string MARIO_STAND_BABY_DOWN_RIGHT = "m_bm_stand_downRight";
    private const string MARIO_STAND_BABY_DOWN_LEFT  = "m_bm_stand_downLeft";
    private const string MARIO_STAND_BABY_UP_RIGHT   = "m_bm_stand_upRight";
    private const string MARIO_STAND_BABY_UP_LEFT    = "m_bm_stand_upLeft";
    
    private const string MARIO_WALK_BABY_UP          = "m_bm_walk_up";
    private const string MARIO_WALK_BABY_DOWN        = "m_bm_walk_down";
    private const string MARIO_WALK_BABY_LEFT        = "m_bm_walk_left";
    private const string MARIO_WALK_BABY_RIGHT       = "m_bm_walk_right";
    
    private const string MARIO_WALK_BABY_DOWN_RIGHT  = "m_bm_walk_downRight";
    private const string MARIO_WALK_BABY_DOWN_LEFT   = "m_bm_walk_downLeft";
    private const string MARIO_WALK_BABY_UP_RIGHT    = "m_bm_walk_upRight";
    private const string MARIO_WALK_BABY_UP_LEFT     = "m_bm_walk_upLeft";

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
        
        moveAngle = Mathf.Rad2Deg * Mathf.Atan2(forceMove.z, forceMove.x) + 180;
        if (forceMove == Vector3.zero)
        {
            moveAngle = 0;
        }
        else
        {
            prevMoveAngle = moveAngle;
        }
        
        SetAnimation();
    }

    private void SetAnimation()
    {
        if (moveAngle != 0)
        {
            if (moveAngle < 22.5 || moveAngle > 337.5) {
                animator.Play(MARIO_WALK_BABY_LEFT);
            } else if (moveAngle < 67.5) {
                animator.Play(MARIO_WALK_BABY_DOWN_LEFT);
            } else if (moveAngle < 112.5) {
                animator.Play(MARIO_WALK_BABY_DOWN);
            } else if (moveAngle < 157.5) {
                animator.Play(MARIO_WALK_BABY_DOWN_RIGHT);
            } else if (moveAngle < 202.5) {
                animator.Play(MARIO_WALK_BABY_RIGHT);
            } else if (moveAngle < 247.5) {
                animator.Play(MARIO_WALK_BABY_UP_RIGHT);
            } else if (moveAngle < 292.5) {
                animator.Play(MARIO_WALK_BABY_UP);
            } else if (moveAngle <= 337.5) {
                animator.Play(MARIO_WALK_BABY_UP_LEFT);
            }
        }
        else
        {
            if (prevMoveAngle < 22.5 || prevMoveAngle > 337.5) {
                animator.Play(MARIO_STAND_BABY_LEFT);
            } else if (prevMoveAngle < 67.5) {
                animator.Play(MARIO_STAND_BABY_DOWN_LEFT);
            } else if (prevMoveAngle < 112.5) {
                animator.Play(MARIO_STAND_BABY_DOWN);
            } else if (prevMoveAngle < 157.5) {
                animator.Play(MARIO_STAND_BABY_DOWN_RIGHT);
            } else if (prevMoveAngle < 202.5) {
                animator.Play(MARIO_STAND_BABY_RIGHT);
            } else if (prevMoveAngle < 247.5) {
                animator.Play(MARIO_STAND_BABY_UP_RIGHT);
            } else if (prevMoveAngle < 292.5) {
                animator.Play(MARIO_STAND_BABY_UP);
            } else if (prevMoveAngle <= 337.5) {
                animator.Play(MARIO_STAND_BABY_UP_LEFT);
            }
        }
    }
}
