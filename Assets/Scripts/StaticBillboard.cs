using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBillboard : Billboard
{
    // Start is called before the first frame update
    void Start()
    {
        Init(gameObject);
    }

    protected override void SetAnimation()
    {
        // Don't
    }
}
