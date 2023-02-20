using UnityEngine;

public class TestNPC : Billboard
{
    // Animation States
    [SerializeField] private string STAND = "old_toad_stand";
    [SerializeField] private string WALK  = "old_toad_walk";
    [SerializeField] private GameObject child;

    private void Start()
    {
        Init(child);
    }

    protected override void SetAnimation()
    {
        _cAnimator.Play(STAND + _facing);
    }
}
