using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MovingObject
{
    public void Move (Vector3 vector3)
    {
        base.AttemptMove ((int) vector3.x, (int) vector3.y);
    }
}