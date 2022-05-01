using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChar : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform parentRot;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Quaternion rot = transform.rotation;
        
        transform.rotation = cam.transform.rotation;
        
        transform.eulerAngles = new Vector3(rot.x, transform.rotation.y, rot.z);
    }

    string SetFacing(float moveAngle)
    {
        if (moveAngle < 22.5 || moveAngle > 337.5)  return "_down";
        if (moveAngle < 67.5)   return "_downLeft";
        if (moveAngle < 112.5)  return "_left";
        if (moveAngle < 157.5)  return "_upLeft";
        if (moveAngle < 202.5)  return "_up";
        if (moveAngle < 247.5)  return "_upRight";
        if (moveAngle < 292.5)  return "_right";
        if (moveAngle <= 337.5) return "_downRight";

        return null;
    }
}
