using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Model : IModel
{
    public Vector3ReactiveProperty Vec3MoveValueRP { get; set; } = new Vector3ReactiveProperty();
    public void ChangeVec3 (Vector3 Vec3)
    {
        Vec3MoveValueRP.Value = Vec3;
    }
}
