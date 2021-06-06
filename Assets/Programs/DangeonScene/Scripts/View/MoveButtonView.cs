using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveButtonView : MonoBehaviour
{
    [SerializeField]
    private ObservableEventTrigger eventTrigger;
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

    public IObservable<PointerEventData> movebutton_OnDown () => eventTrigger.OnPointerDownAsObservable ();
    public IObservable<PointerEventData> movebutton_OnUp () => eventTrigger.OnPointerUpAsObservable ();

}