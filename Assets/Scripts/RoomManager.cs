using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

enum Directions
{
    Right,
    Left,
    Up,
    Down,
    DownLeft,
    DownRight,
    UpLeft,
    UpRight
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private string _destination;
    [SerializeField] private Directions _movementDirection = Directions.Right;

    private Vector3 _destVector = Vector3.zero;

    private Transform[] _players = new Transform[2];
    private GameObject _pathways;

    private bool _debounce = false;
    private bool _triggered = false;

    private static string _lastScene = "";

    private void Start()
    {
        switch (_movementDirection)
        {
            case Directions.Down:
                _destVector = new Vector3(0f, 0f, -4);
                break;
            case Directions.Up:
                _destVector = new Vector3(0f, 0f, 4f);
                break;
            case Directions.Right:
                _destVector = new Vector3(4f, 0f, 0f);
                break;
            case Directions.Left:
                _destVector = new Vector3(-4f, 0f, 0f);
                break;
            case Directions.DownLeft:
                _destVector = new Vector3(-4f, 0f, -4);
                break;
            case Directions.DownRight:
                _destVector = new Vector3(4f, 0f, -4f);
                break;
            case Directions.UpLeft:
                _destVector = new Vector3(-4f, 0f, 4f);
                break;
            case Directions.UpRight:
                _destVector = new Vector3(4f, 0f, 4f);
                break;
        }
        
        _players[0] = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        _players[1] = GameObject.FindGameObjectsWithTag("Player")[1].transform;

        _pathways = GameObject.FindWithTag("Pathways");

        for(int i = 0; i < _pathways.transform.childCount; i++)
        {
            
            Transform pathway = _pathways.transform.GetChild(i);

            if(_destination == _lastScene)
            {
                StartCoroutine(OnDebounce());
                _lastScene = "";
                
                _players[0].position = pathway.position + _destVector;
                _players[1].position = pathway.position + _destVector;

                _players[0].GetComponent<CutsceneController>().MoveToTarget(transform.position - _destVector);
                _players[1].GetComponent<CutsceneController>().MoveToTarget(transform.position - _destVector);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_debounce) {
            if(other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(LoadSceneTransition(_destination));
            }
        }
    }

    private IEnumerator LoadSceneTransition(string sceneName)
    {
        _transition.SetTrigger("Start");

        _players[0].GetComponent<CutsceneController>().MoveToTarget(transform.position + _destVector);
        _players[1].GetComponent<CutsceneController>().MoveToTarget(transform.position + _destVector);

        yield return new WaitForSeconds(_transitionTime);
        
        Scene scene = SceneManager.GetActiveScene();
        _lastScene = scene.name;
        
        SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator OnDebounce()
    {
        _debounce = true;

        yield return new WaitForSeconds(5f);

        _debounce = false;
    }
}