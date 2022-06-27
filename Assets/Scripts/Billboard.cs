using UnityEngine;

public abstract class Billboard : MonoBehaviour
{
    // Sprites
    protected float moveAngle;
    protected float prevMoveAngle;
    protected string facing = "_down";
    protected Transform cam;
    protected Transform sprite;
    protected Animator animator;

    protected void Init(GameObject child)
    {
        sprite = child.transform;
        animator = child.GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    protected void LateUpdate()
    {
        facing = SetFacing(cam.eulerAngles.y - transform.eulerAngles.y);
        
        SetAnimation();
        
        Quaternion rot = sprite.transform.rotation;
        
        sprite.transform.rotation = cam.rotation;
        
        sprite.transform.rotation = Quaternion.Euler(rot.eulerAngles.x, sprite.transform.rotation.eulerAngles.y, rot.eulerAngles.z);
    }
    
    private string SetFacing(float moveAngle)
    {
        while (moveAngle < 0)
        {
            moveAngle += 360;
        }
        
        if (moveAngle < 22.5 || moveAngle > 337.5)  return "_u";
        if (moveAngle < 67.5)   return "_ul";
        if (moveAngle < 112.5)  return "_l";
        if (moveAngle < 157.5)  return "_dl";
        if (moveAngle < 202.5)  return "_d";
        if (moveAngle < 247.5)  return "_dr";
        if (moveAngle < 292.5)  return "_r";
        if (moveAngle <= 337.5) return "_ur";

        return null;
    }
    
    protected abstract void SetAnimation();
}
