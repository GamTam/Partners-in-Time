using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController
{
    

    /*

    private void Update()
    {
        Vector3 forceMove = new Vector3(_moveVector.ReadValue<Vector2>().x, 0, _moveVector.ReadValue<Vector2>().y).normalized;
        Vector3 newMove = new Vector3(0f, 0f, 0f);

        if (forceMove.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(forceMove.x, forceMove.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            newMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            MoveAngle = targetAngle;
            
            _controller.Move(newMove * moveSpeed * Time.deltaTime);
        }

        if (!_controller.isGrounded)
        {
            activeGrav = activeGrav < maxGrav ? activeGrav + gravSpeed * Time.deltaTime : maxGrav;
            _controller.Move(new Vector3(0, -activeGrav, 0f));
        }
        else
        {
            activeGrav = 0;
        }

        if (forceMove == Vector3.zero)
        {
            MoveAngle = float.MinValue;
        }
        else
        {
            PrevMoveAngle = MoveAngle;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, PrevMoveAngle, transform.eulerAngles.z);
    }
    
    protected override void SetAnimation()
    {
        if (MoveAngle != float.MinValue)
        {
            Animator.Play(MarioWalk + Facing);
        }
        else
        {
            Animator.Play(MarioStand + Facing);
        }
    }*/
}
