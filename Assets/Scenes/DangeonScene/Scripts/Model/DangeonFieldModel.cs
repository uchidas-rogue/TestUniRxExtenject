using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DangeonFieldModel : IDangeonFieldModel
{
    public IntReactiveProperty FloorNumRP { get; set; } = new IntReactiveProperty (1);
    public bool IsFieldSetting { get; set; }
    public int[, , ] Field { get; set; }
}