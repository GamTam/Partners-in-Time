using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionManager
{
    // Enemy
    private GameObject _enemy;
    private CharacterController _controller;

    // Movement
    private bool _isMoving = false;

    // Getters
    public bool IsMoving { get { return _isMoving; } }

    public EnemyActionManager(GameObject enemy) {
        _enemy = enemy;
        _controller = _enemy.GetComponent<CharacterController>();
    }

    public Vector3 GetMoveVector(Vector3 target) {
        Vector3 moveVector = target - _enemy.transform.position;

        if(moveVector.magnitude > 0.7f) {
            _isMoving = true;
            moveVector = moveVector.normalized;
        } else {
            moveVector = moveVector.normalized / 20f;
            _isMoving = false;
        }

        return moveVector;
    }
} 
