using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _textBoxPrefab;
    private GameObject _tempBox;
    private TMP_Text _advanceButton;
    private TMP_Text textBox;
    public AudioClip typingClip;
    //public AudioSourceGroup audioSourceGroup;
    private Queue<string> lines;
    private PlayerInput _playerInput;

    private DialogueVertexAnimator dialogueVertexAnimator;
    private bool movingText;
    private InputAction _advanceText;
    void Awake() {
        lines = new Queue<string>();
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        _advanceText = _playerInput.actions["confirm"];
    }

    public void StartText(String[] linesIn)
    {
        _tempBox = Instantiate(_textBoxPrefab);
        _tempBox.transform.SetParent(GameObject.FindWithTag("UI").transform, false);

        TMP_Text[] texts = _tempBox.GetComponentsInChildren<TMP_Text>();

        textBox = texts[0];
        _advanceButton = texts[1];
        _advanceButton.enabled = false;
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox/*, audioSourceGroup*/);
        
        
        _playerInput.SwitchCurrentActionMap("Menu");
        lines.Clear();
        
        foreach (string line in linesIn)
        {
            lines.Enqueue(line);
        }
        
        NextLine();
    }

    private void Update()
    {
        if (_advanceText.triggered)
        {
            NextLine();
        }

        if (!dialogueVertexAnimator.textAnimating)
        {
            _advanceButton.enabled = true;
        }
    }

    private Coroutine typeRoutine = null;
    public void NextLine() {
        if (dialogueVertexAnimator.textAnimating)
        {
            dialogueVertexAnimator.QuickEnd();
            return;
        }
        
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (movingText)
        {
            return;
        }

        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands =
            DialogueUtility.ProcessInputString(lines.Dequeue(), out string totalTextMessage);
        TextAlignOptions[] textAlignInfo = SeparateOutTextAlignInfo(commands);
        String nameInfo = SeparateOutNameInfo(commands);

        for (int i = 0; i < textAlignInfo.Length; i++)
        {
            TextAlignOptions info = textAlignInfo[i];
            if (info == TextAlignOptions.topCenter)
            {
                textBox.alignment = TextAlignmentOptions.Top;
            }
            else if (info == TextAlignOptions.midCenter)
            {
                textBox.alignment = TextAlignmentOptions.Center;
            }
            else if (info == TextAlignOptions.left)
            {
                textBox.alignment = TextAlignmentOptions.TopLeft;
            }
            else if (info == TextAlignOptions.right)
            {
                textBox.alignment = TextAlignmentOptions.TopRight;
            }
        }

        _advanceButton.enabled = false;
        typeRoutine =
            StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
    }
    
    private TextAlignOptions[] SeparateOutTextAlignInfo(List<DialogueCommand> commands) {
        List<TextAlignOptions> tempResult = new List<TextAlignOptions>();
        for (int i = 0; i < commands.Count; i++) {
            DialogueCommand command = commands[i];
            if (command.type == DialogueCommandType.Align) {
                tempResult.Add(command.textAlignOptions);
            }
        }
        return tempResult.ToArray();
    }
    
    private String SeparateOutNameInfo(List<DialogueCommand> commands) {
        for (int i = 0; i < commands.Count; i++) {
            DialogueCommand command = commands[i];
            if (command.type == DialogueCommandType.Name) {
                return command.stringValue;
            }
        }
        return null;
    }

    void EndDialogue()
    {
        StartCoroutine(dialogueVertexAnimator.AnimateTextIn(new List<DialogueCommand>(), "", typingClip, null));
        Destroy(_tempBox);
        _playerInput.SwitchCurrentActionMap("Overworld");
    }
}
