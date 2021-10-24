using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx.Triggers;

public class AttackButtonView : MonoBehaviour
{
    ObservableEventTrigger _eventTrigger;

    void Awake()
    {
        _eventTrigger = GetComponent<ObservableEventTrigger>();
    }

    // click event
    public IObservable<PointerEventData> OnClick()=>_eventTrigger.OnPointerClickAsObservable();
}