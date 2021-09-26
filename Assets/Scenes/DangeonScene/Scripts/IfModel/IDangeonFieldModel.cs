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
    /// フィールド
    /// </summary>
    /// <value></value>
    FieldClass[, ] Field { get; set; }
    /// <summary>
    /// ミニマップ
    /// </summary>
    /// <value></value>
    MapClass[, ] Map { get; set; }
    /// <summary>
    /// アイテム
    /// </summary>
    /// <value></value>
    ItemClass[, ] Item { get; set; }
}