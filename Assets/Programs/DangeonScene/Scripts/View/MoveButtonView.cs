using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveButtonView : MonoBehaviour
{
    [SerializeField]
    private ObservableEventTrigger EventTrigger;
    /// <summary>
    /// 移動方向を示す数値 水平方向
    /// </summary>
    [SerializeField]
    public float VectorX;
    /// <summary>
    /// 移動方向を示す数値 垂直方向
    /// </summary>
    [SerializeField]
    public float VectorY;

    public IObservable<PointerEventData> movebutton_OnDown () => EventTrigger.OnPointerDownAsObservable ();
    public IObservable<PointerEventData> movebutton_OnUp () => EventTrigger.OnPointerUpAsObservable ();

}