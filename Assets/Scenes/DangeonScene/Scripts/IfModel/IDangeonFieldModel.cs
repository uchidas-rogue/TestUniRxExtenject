using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IDangeonFieldModel
{
    /// <summary>
    /// 現在のダンジョンの階層
    /// </summary>
    /// <value></value>
    IntReactiveProperty FloorNumRP { get; set; }
    /// <summary>
    /// フィールの設定中かどうか
    /// </summary>
    /// <value></value>
    bool IsFieldSetting { get; set; }
    /// <summary>
    /// x,y,z z=0=>field z=1=>map
    /// </summary>
    /// <value></value>
    int[, , ] Field { get; set; }
}