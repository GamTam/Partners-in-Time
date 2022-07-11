using UnityEngine;

public class AreaTriggers : MonoBehaviour
{
    public GameObject virtualCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !(other.isTrigger))
        {
            virtualCam.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !(other.isTrigger))
        {
            virtualCam.SetActive(false);
        }
    }
}
