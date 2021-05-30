using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IPlayerModel
{
    Vector3ReactiveProperty PlayerInputVec3RP { get; set; }
    BoolReactiveProperty IsPlayerMovingRP { get; set; }
    ReactiveProperty<Direction> DirectionPlayerRP { get; set; }
    Vector3ReactiveProperty PlayerPositionVec3RP { get; set; }
    void ChangeVec3 (float x, float y);
    void InitInputVec ();
}