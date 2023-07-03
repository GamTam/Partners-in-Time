using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool _localize;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [TextArea(15, 20)] [SerializeField] private string[] _dialogue;
    
    private LocalizedStringTable _stringTable = new LocalizedStringTable { TableReference = "StringTranslations" };

    private static MarioOverworldStateMachine _mar;
    private bool _talkable = false;
    
    public bool Talkable {get {return _talkable;} set {_talkable = value;}}

    private void Update()
    {
        if (_talkable && Globals.Mario.MAction)
        {
            TriggerDialogue();
        }
    }
    
    static string GetLocalizedString(StringTable table, string entryName)
    {
        // Get the table entry. The entry contains the localized string and Metadata
        var entry = table.GetEntry(entryName);
        
        string line = entry == null? "No translation found for key '" + entryName + "' in StringTranslations" : entry.GetLocalizedString();
        return line; // We can pass in optional arguments for Smart Format or String.Format here.
    }

    public void TriggerDialogue()
    {
        string[] dialogue = (string[]) _dialogue.Clone();
        
        if (_localize)
        {
            for (int i=0; i < dialogue.Length; i++)
            {
                dialogue[i] = GetLocalizedString(_stringTable.GetTable(), _dialogue[i]);
            }
        }
        
        FindObjectOfType<DialogueManager>().StartText(dialogue, gameObject.transform, _spriteRenderer);
    }
}
