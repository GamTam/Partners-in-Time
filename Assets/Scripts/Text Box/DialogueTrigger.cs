using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(15, 20)] [SerializeField] private String[] _dialogue;
    private static MarioOverworldStateMachine _mar;
    private bool _talkable = false;
    
    public bool Talkable {get {return _talkable;} set {_talkable = value;}}

    private void Awake()
    {
        if (_mar == null)
        {
            _mar = GameObject.FindWithTag("Player").GetComponent<MarioOverworldStateMachine>();
        }
    }

    private void Update()
    {
        if (_talkable && _mar.Action)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartText(_dialogue, gameObject.transform);
    }
}
