using System;
using System.Collections;
using System.Collections.Generic;
using Gravitons.UI.Modal;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyModalManager : ModalManager
{
    ObservableEventTrigger _eventTrigger;
    Image _modalBack;

    void Awake ()
    {
        _eventTrigger = GetComponent<ObservableEventTrigger> ();
        _modalBack = GetComponent<Image> ();
    }

    void Start ()
    {
        
    }
}