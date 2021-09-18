using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IPlayerModel
{
    /// <summary>
    /// playerへの移動入力
    /// </summary>
    /// <value></value>
    Vector3ReactiveProperty PlayerInputVec3RP { get; set; }
    /// <summary>
    /// playerの移動方向
    /// </summary>
    /// <value></value>
    ReactiveProperty<Direction> DirectionPlayerRP { get; set; }
    /// <summary>
    /// Playerの現在位置
    /// </summary>
    /// <value></value>
    Vector3ReactiveProperty PlayerPositionVec3RP { get; set; }
}