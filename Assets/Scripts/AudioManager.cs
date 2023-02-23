using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject _me;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_me)
        {
            Destroy(gameObject);
            return;
        }
        
        _me = gameObject;
    }
}
