using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private string _destination;

    private Transform[] _players = new Transform[2];
    private GameObject _pathways;

    private bool _debounce = false;
    private bool _triggered = false;

    private static string _lastScene = "";

    public string Destination { get { return _destination; }}

    private void Start()
    {
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

        _players[0].GetComponent<CharCutsceneInput>().MoveToTarget(transform.position + new Vector3(20f, 0f, 0f));
        
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