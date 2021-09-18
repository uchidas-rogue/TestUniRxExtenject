using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IMiniMapModel
{
    /// <summary>
    /// minimapが拡大表示状態かどうか
    /// </summary>
    /// <value></value>
    bool IsPickup { get; set; }
    /// <summary>
    /// 拡大時のミニマップの位置
    /// </summary>
    /// <value></value>
    Vector3 PickedMapPositionVec3 { get; set; }
        /// <summary>
    /// 拡大時のミニマップの大きさ
    /// </summary>
    /// <value></value>
    Vector2 PiciedMapSizeVec2 { get; set; }
    /// <summary>
    /// 通常時のミニマップの位置
    /// </summary>
    /// <value></value>ï
    Vector3 MiniMapPositionVec3 { get; set; }
    /// <summary>
    /// 通常時のミニマップの大きさ
    /// </summary>
    /// <value></value>
    Vector2 MiniMapSizeVec2 { get; set; }
}