using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class MiniMapModel : IMiniMapModel
{
    public BoolReactiveProperty isPickupRP { get; set; } = new BoolReactiveProperty (false);
}