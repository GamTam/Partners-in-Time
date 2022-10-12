using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;

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
    [SerializeField] private Directions _transDir = Directions.Right;

    private Vector3 _destVector = Vector3.zero;

    private Transform[] _players = new Transform[2];
    private GameObject _pathways;

    private bool _debounce = false;
    private bool _triggered = false;

    private static string _lastScene = "";

    public string Destination { get { return _destination; }}

    private void Start()
    {
        switch (_transDir)
        {
            case Directions.Down:
                _destVector = new Vector3(0f, 0f, -20);
                break;
            case Directions.Up:
                _destVector = new Vector3(0f, 0f, 20f);
                break;
            case Directions.Right:
                _destVector = new Vector3(20f, 0f, 0f);
                break;
            case Directions.Left:
                _destVector = new Vector3(-20f, 0f, 0f);
                break;
            case Directions.DownLeft:
                _destVector = new Vector3(-20f, 0f, -20);
                break;
            case Directions.DownRight:
                _destVector = new Vector3(20f, 0f, -20f);
                break;
            case Directions.UpLeft:
                _destVector = new Vector3(-20f, 0f, 20);
                break;
            case Directions.UpRight:
                _destVector = new Vector3(20f, 0f, 20f);
                break;
        }
        
        _players[0] = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        _players[1] = GameObject.FindGameObjectsWithTag("Player")[1].transform;

        _pathways = GameObject.FindWithTag("Pathways");

        for(int i = 0; i < _pathways.transform.childCount; i++)
        {
            
            Transform pathway = _pathways.transform.GetChild(i);

            if(pathway.GetComponent<RoomManager>().Destination == _lastScene)
            {
                StartCoroutine(OnDebounce());
                
                // _players[0].position = pathway.position;
                // _players[1].position = pathway.position;
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

        _players[0].GetComponent<CharCutsceneInput>().MoveToTarget(transform.position + _destVector);
        
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