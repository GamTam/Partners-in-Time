using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;

    private InputAction _start;
    
    private void Start()
    {
        _playerInput = GameObject.FindWithTag("MainCamera").GetComponent<PlayerInput>();
        _start = _playerInput.actions["jump"];
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().Equals("TestRoom"))
        {
            StartCoroutine(Kill());
        }
        if (_start.triggered)
        {
            AudioManager.FadeoutAll();
            StartCoroutine(LoadSceneTransition("TestRoom"));
        }
    }

    IEnumerator LoadSceneTransition(string sceneName)
    {
        _transition.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(_transitionTime);
        Destroy(gameObject);
    }
}
