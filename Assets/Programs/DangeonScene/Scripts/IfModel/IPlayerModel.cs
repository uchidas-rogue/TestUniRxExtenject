using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IPlayerModel
{
    Vector3ReactiveProperty Vec3PlayerPositionRP { get; set; }
    BoolReactiveProperty IsPlayerTurnRP { get; set; }
    void ChangeVec3 (float x, float y);
    bool CheckKeyInput ();
}