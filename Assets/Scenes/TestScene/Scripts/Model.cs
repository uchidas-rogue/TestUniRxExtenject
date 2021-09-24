using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Model : IModel
{
    public Vector3ReactiveProperty Vec3MoveValueRP { get; set; } = new Vector3ReactiveProperty();
    public void ChangeVec3 (float x,float y)
    {
        Vec3MoveValueRP.Value += new Vector3(x,y);
    }

    public bool CheckKeyInput(float x,float y)
    {
        return x != 0 || y != 0 ;
    }
}
