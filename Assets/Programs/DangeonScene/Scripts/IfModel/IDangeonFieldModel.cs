using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IDangeonFieldModel
{
    IntReactiveProperty FloorNumRP { get; set; }
    bool IsFieldSetting { get; set; }
    /// <summary>
    /// x,y,z z=0=>field z=1=>map
    /// </summary>
    /// <value></value>
    int[, , ] Field { get; set; }

    void MakeField (int width, int height, int level);
}