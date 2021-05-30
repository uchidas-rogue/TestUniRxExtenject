using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerView : MovingObject
{

    private Vector3 InitPosVec3 = new Vector3 (1f, 1f, 0);
    public void Move (Vector3 vector3)
    {
        base.AttemptMove ((int) vector3.x, (int) vector3.y);
    }

    public void InitPosition ()
    {
        base.transformCash.position = InitPosVec3;
    }
}