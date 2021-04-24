using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IDangeonFieldModel
{
    public IntReactiveProperty FloorNumRP { get; set; }
    public int[, , ] Field { get; set; }

    public void MakeField (int width, int height);
}