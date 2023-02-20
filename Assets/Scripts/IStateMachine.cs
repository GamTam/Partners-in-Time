using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    void SetOnCutscene(bool value);
    void SetCMoveVector(Vector2 value);
}
