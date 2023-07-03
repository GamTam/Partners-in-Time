using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static float DeadZone = 0.1f;
    public static MarioOverworldStateMachine Mario;

    public static object MusicManager { get; internal set; }
    public static object Soundtrack { get; internal set; }
    public static SoundManager SoundManager { get; internal set; }

    internal static Dictionary<string, ArrayList> LoadTSV(TextAsset tsvHandler)
    {
        throw new NotImplementedException();
    }
}