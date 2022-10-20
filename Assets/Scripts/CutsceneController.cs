using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private IStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = GetComponent(typeof(IStateMachine)) as IStateMachine;
    }

    public void MoveToTarget(Vector3 target)
    {
        _stateMachine.SetOnCutscene(true);
        
        StartCoroutine(OnMove(target));
    }

    private IEnumerator OnMove(Vector3 target)
    {
        while(true)
        {
            Vector3 offset = target - transform.position;
            
            if(offset.magnitude > 5f)
            {
                offset = offset.normalized;

                _stateMachine.SetCMoveVector(new Vector2(offset.x, offset.z));
            } else {
                _stateMachine.SetOnCutscene(false);
            }

            yield return null;
        }
    }
}
