using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonView : MonoBehaviour
{
    [SerializeField]
    public Button button;
    /// <summary>
    /// 移動方向を示す数値 水平方向
    /// </summary>
    [SerializeField]
    public float vectorX;
    /// <summary>
    /// 移動方向を示す数値 垂直方向
    /// </summary>
    [SerializeField]
    public float vectorY;

    public IObservable<Unit> button_OnClick () => button.onClick.AsObservable ();
}