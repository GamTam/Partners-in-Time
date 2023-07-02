using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    
    public float volume = 1;
    public float pitch = 1;

    public AudioSource source;
}
