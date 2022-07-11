using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cloud;

    [SerializeField] private int _maxCloudTime = 5;
    [SerializeField] private int _cloudFrequency = 5;
    private float _timer = 0f;

    private System.Random _rand = new System.Random();
    
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _maxCloudTime || _rand.Next(_cloudFrequency) == 0)
        {
            _timer = 0f;
            Instantiate(_cloud, transform);
        }
    }
}
