using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : Billboard
{
    // Animation States
    private const string STAND = "mnt_todd_stand";
    private const string WALK  = "mnt_todd_walk";
    [SerializeField] private GameObject child;

    private void Start()
    {
        Init(child);
    }

    protected override void SetAnimation()
    {
        _animator.Play(STAND + _facing);
    }
}
