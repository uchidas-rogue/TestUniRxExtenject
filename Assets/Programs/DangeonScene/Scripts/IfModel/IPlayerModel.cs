using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IPlayerModel
{
    Vector3ReactiveProperty Vec3PlayerPositionRP { get; set; }
    BoolReactiveProperty IsPlayerMovingRP { get; set; }
    ReactiveProperty<Direction> DirectionPlayerRP { get; set; }
    void ChangeVec3 (float x, float y);
}