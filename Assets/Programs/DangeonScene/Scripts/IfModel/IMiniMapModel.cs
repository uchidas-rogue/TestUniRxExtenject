using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IMiniMapModel
{
    BoolReactiveProperty isPickupRP { get; set; }
}