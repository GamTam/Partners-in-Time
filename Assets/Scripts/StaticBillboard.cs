using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBillboard : Billboard
{
    [SerializeField] private int _layerMaskOffset = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Init(gameObject);
        _offset = _layerMaskOffset;
    }

    protected override void SetAnimation()
    {
        // Don't
    }
}
