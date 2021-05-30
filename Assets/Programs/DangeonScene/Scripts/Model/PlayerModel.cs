using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerModel : IPlayerModel
{
    public Vector3ReactiveProperty PlayerInputVec3RP { get; set; } = new Vector3ReactiveProperty ();
    public BoolReactiveProperty IsPlayerMovingRP { get; set; } = new BoolReactiveProperty (false);
    public ReactiveProperty<Direction> DirectionPlayerRP { get; set; } = new ReactiveProperty<Direction> (Direction.none);
    public Vector3ReactiveProperty PlayerPositionVec3RP { get; set; } = new Vector3ReactiveProperty ();

    private Vector3 vector3 = new Vector3 (0f, 0f, 0f);

    public void ChangeVec3 (float x, float y)
    {
        IsPlayerMovingRP.Value = true;
        InitInputVec ();

        if (x != 0) x = x > 0f ? 1f : -1f;
        if (y != 0) y = y > 0f ? 1f : -1f;

        DirectionPlayerRP.Value = CheckDirection (x, y);

        vector3.Set (x, y, 0f);
        PlayerInputVec3RP.Value = vector3;

        IsPlayerMovingRP.Value = false;
    }

    public void InitInputVec ()
    {
        vector3.Set (0f, 0f, 0f);
        PlayerInputVec3RP.Value = vector3;
    }

    public Direction CheckDirection (float x, float y)
    {
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