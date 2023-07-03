using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectTriggers : MonoBehaviour
{
    public void PlaySound(string sound)
    {
        Globals.SoundManager.Play(sound);
    }
}
