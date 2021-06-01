using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonView : MonoBehaviour
{
    [SerializeField]
    public Button movebutton;
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

    public IObservable<Unit> movebutton_OnClick () => movebutton.onClick.AsObservable ();
}