using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveObjectServece
{
    Vector3 GetInputVec (float x, float y);
    Direction GetInputDirection (float x, float y);
}

public class MoveObjectServece : IMoveObjectServece
{
    Vector3 _vector3 = new Vector3 ();

    public Vector3 GetInputVec (float x, float y)
    {
        if (x != 0) x = x > 0f ? 1f : -1f;
        if (y != 0) y = y > 0f ? 1f : -1f;

        _vector3.Set (x, 0f, y);
        return _vector3;
    }

    public Direction GetInputDirection (float x, float y)
    {
        if (x != 0) x = x > 0f ? 1f : -1f;
        if (y != 0) y = y > 0f ? 1f : -1f;

        switch (x)
        {
            case 0:
                switch (y)
                {
                    case 1: // 0,1
                        return Direction.up;
                    case -1: // 0,-1
                        return Direction.down;
                }
                break;
            case 1:
                switch (y)
                {
                    case 0: // 1,0
                        return Direction.right;
                    case 1: // 1,1
                        return Direction.upright;
                    case -1: // 1,-1
                        return Direction.downright;
                }
                break;
            case -1:
                switch (y)
                {
                    case 0: // -1,0
                        return Direction.left;
                    case 1: // -1,1
                        return Direction.upleft;
                    case -1: // -1,-1
                        return Direction.downleft;
                }
                break;
        }
        return Direction.none;
    }
}