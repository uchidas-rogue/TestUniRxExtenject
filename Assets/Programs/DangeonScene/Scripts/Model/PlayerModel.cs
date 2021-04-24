using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerModel : IPlayerModel
{
    public Vector3ReactiveProperty Vec3PlayerPositionRP { get; set; } = new Vector3ReactiveProperty ();
    public BoolReactiveProperty IsPlayerMovingRP { get; set; } = new BoolReactiveProperty (false);

    private Vector3 vector3 = new Vector3 (0f, 0f, 0f);

    public void ChangeVec3 (float x, float y)
    {
        IsPlayerMovingRP.Value = true;
        vector3.Set (0f, 0f, 0f);
        Vec3PlayerPositionRP.Value = vector3;

        if (x != 0) x = x > 0f ? 1f : -1f;
        if (y != 0) y = y > 0f ? 1f : -1f;
        vector3.Set (x, y, 0f);
        Vec3PlayerPositionRP.Value = vector3;

        IsPlayerMovingRP.Value = false;
    }
}