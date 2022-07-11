using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private PlayerInput _playerInput;
    [SerializeField] private InputAction inputActions;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GameObject.FindWithTag("MainCamera").GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.SetText("Press <sprite=\"" + _playerInput.currentControlScheme + "\" name=\"" 
            + _playerInput.actions["Confirm"].GetBindingDisplayString()+ 
            "\"> To Start");
    }
}
