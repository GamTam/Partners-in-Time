using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCutsceneInput : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Transform _cam;

    private void Awake()
    {
        _controller = transform.GetComponent<CharacterController>();
        _animator = transform.GetComponent<Animator>();
        _cam = Camera.main.transform;
    }

    public void MoveToTarget(Vector3 target)
    {
        transform.GetComponent<MarioOverworldStateMachine>().OnCutscene = true;
        
        StartCoroutine(OnMove(target));
    }

    private IEnumerator OnMove(Vector3 target)
    {
        while(true)
        {
            Vector3 offset = target - transform.position;
            Debug.Log(target + " : " + transform.position);

            if(offset.magnitude > 0.1f)
            {
                offset = offset.normalized;

                transform.GetComponent<MarioOverworldStateMachine>().MoveVector = new Vector2(offset.x, offset.z);
            }

            yield break;
        }
    }
}
