using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static FMOD.Studio.Bus _masterBus;
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
        _masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    // Update is called once per frame
    public static void FadeoutAll()
    {
        _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
