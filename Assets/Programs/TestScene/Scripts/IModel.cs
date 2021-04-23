using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IModel
{
    Vector3ReactiveProperty Vec3MoveValueRP { get; set; }
    void ChangeVec3 (float x,float y);
    public bool CheckKeyInput(float x,float y);
}
