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

    public Vector3 MoveTowardsTarget(Vector3 target, float moveSpeed) {
        Vector3 moveVector = Vector3.zero;
        Vector3 offset = target - _enemy.transform.position;

        if(offset.magnitude > 0.7f) {
            _isMoving = true;
            moveVector = offset.normalized;
            //ebug.Log(offset.normalized.z);
            offset = offset.normalized * moveSpeed;
            //_controller.Move(offset * Time.deltaTime);
        } else {
            moveVector = offset.normalized / 20f;
            _isMoving = false;
        }

        return moveVector;
    }
} 
