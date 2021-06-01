using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MiniMapModel : IMiniMapModel
{
    public BoolReactiveProperty IsPickupRP { get; set; } = new BoolReactiveProperty (false);
    public Vector3 PickedMapPositionVec3 { get; set; } = new Vector3 (0f, 0f, 0f);
    public Vector2 PiciedMapSizeVec2 { get; set; } = new Vector2 (2220f, 1040f);
    public Vector3 MiniMapPositionVec3 { get; set; } = new Vector3 (720f, 320f, 0f);
    public Vector2 MiniMapSizeVec2 { get; set; } = new Vector2 (800f, 420f);
}