using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ControllerManager : MonoBehaviour
{
    private static GameObject _me;
    [SerializeField] private VariablesGroupAsset _source;
    private PlayerInput _playerInput;
    private string _currentPlayerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_me)
        {
            Destroy(gameObject);
            return;
        }

        _playerInput = GetComponent<PlayerInput>();
        _currentPlayerInput = _playerInput.currentControlScheme;
        
        _me = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerInput.currentControlScheme.Equals(_currentPlayerInput))
        {
            UpdateButtonPrompts();
        }
    }

    void UpdateButtonPrompts()
    {
        _currentPlayerInput = _playerInput.currentControlScheme;
        
        var activeController = _source["controller"] as StringVariable;
        var confirmButton = _source["confirmbutton"] as StringVariable;
        
        activeController.Value = _playerInput.currentControlScheme;
        confirmButton.Value = _playerInput.actions["Confirm"].GetBindingDisplayString();
    }
}
