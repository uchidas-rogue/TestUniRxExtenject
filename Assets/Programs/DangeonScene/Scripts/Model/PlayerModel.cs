using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerModel : IPlayerModel
{
    public Vector3ReactiveProperty Vec3PlayerPositionRP { get; set; } = new Vector3ReactiveProperty();
    private Vector3 vec3 = new Vector3();
    public void ChangeVec3 (float x,float y)
    {
        vec3.x = x;
        vec3.y = y;
        Vec3PlayerPositionRP.Value += vec3;
    }

    public bool CheckKeyInput(float x,float y)
    {
        return x != 0 || y != 0 ;
    }
}
