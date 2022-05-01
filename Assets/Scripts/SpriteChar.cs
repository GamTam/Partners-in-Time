using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChar : MonoBehaviour
{
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.rotation;
        
        transform.rotation = cam.transform.rotation;
        
        transform.rotation = Quaternion.Euler(rot.eulerAngles.x, transform.eulerAngles.y, rot.eulerAngles.z);
    }
}
