using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IMiniMapModel
{
    bool IsPickup { get; set; }
    Vector3 PickedMapPositionVec3 { get; set; }
    Vector2 PiciedMapSizeVec2 { get; set; }
    Vector3 MiniMapPositionVec3 { get; set; }
    Vector2 MiniMapSizeVec2 { get; set; }
}