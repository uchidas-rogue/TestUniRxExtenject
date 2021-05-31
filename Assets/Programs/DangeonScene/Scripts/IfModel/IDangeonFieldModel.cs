using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IDangeonFieldModel
{
    IntReactiveProperty FloorNumRP { get; set; }
    BoolReactiveProperty IsFieldSetting { get; set; }
    int[, , ] Field { get; set; }

    void MakeField (int width, int height, int level);
}