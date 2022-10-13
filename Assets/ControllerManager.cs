using System;
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

    private int _currentControlInt = 0;
    private string _currentControlerName = "Keyboard";
    
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
        
        UpdateButtonPrompts();
        
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
        var confirmButton = _source["confirm"] as StringVariable;
        var switchAction = _source["switch_action"] as StringVariable;
        var mAction = _source["m_action"] as StringVariable;
        var lAction = _source["l_action"] as StringVariable;
        var jumpAction = _source["jump"] as StringVariable;

        activeController.Value = _playerInput.currentControlScheme;

        switch (_playerInput.currentControlScheme)
        {
            case "Keyboard":
                _currentControlInt = 0;
                _currentControlerName = "Keyboard";
                break;
            case "PS4 Controller":
                _currentControlInt = 1;
                _currentControlerName = "DualShockGamepad";
                break;
            case "Xbox Controller":
                _currentControlInt = 2;
                _currentControlerName = "XInputController";
                break;
        }
        
        Debug.Log($"{_playerInput.actions["confirm"].GetBindingDisplayString()}, {_playerInput.actions["confirm"].bindings[_currentControlInt].ToString().Replace($"confirm:<{_currentControlerName}>/", "").Replace($"[{_playerInput.currentControlScheme}]", "")}");

        confirmButton.Value = _playerInput.actions["confirm"].bindings[_currentControlInt].ToString().Replace($"confirm:<{_currentControlerName}>/", "").Replace($"[{_playerInput.currentControlScheme}]", "");
        switchAction.Value = _playerInput.actions["switch_action"].GetBindingDisplayString();
        mAction.Value = _playerInput.actions["m_action"].GetBindingDisplayString();
        lAction.Value = _playerInput.actions["l_action"].GetBindingDisplayString();
        jumpAction.Value = _playerInput.actions["jump"].bindings[_currentControlInt].ToString().Replace($"jump:<{_currentControlerName}>/", "").Replace($"[{_playerInput.currentControlScheme}]", "");
    }
}
