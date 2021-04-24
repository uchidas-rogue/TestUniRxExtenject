using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IPlayerModel
{
    Vector3ReactiveProperty Vec3PlayerPositionRP { get; set; }
    void ChangeVec3 (float x,float y);
    public bool CheckKeyInput(float x,float y);
}
