using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveButtonView : MonoBehaviour
{
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

    ObservableEventTrigger _eventTrigger;

    void Awake()
    {
        _eventTrigger = GetComponent<ObservableEventTrigger>();
    }

    public IObservable<PointerEventData> movebutton_OnDown () => _eventTrigger.OnPointerDownAsObservable ();
    public IObservable<PointerEventData> movebutton_OnUp () => _eventTrigger.OnPointerUpAsObservable ();

}