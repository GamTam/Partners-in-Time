using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenClockHandles : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    private System.Random _rand = new System.Random();

    private void Awake()
    {
        transform.Rotate(0, 0, _rand.Next(360));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
