using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    [SerializeField] private Transform parentRot;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        Quaternion rot = transform.rotation;
        
        transform.rotation = cam.rotation;
        
        transform.rotation = Quaternion.Euler(rot.eulerAngles.x, transform.rotation.eulerAngles.y, rot.eulerAngles.z);
    }
}
