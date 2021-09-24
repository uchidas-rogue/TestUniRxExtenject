using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ButtonView : MonoBehaviour
{
    [SerializeField]
    Button button;

    public IObservable<Unit> button_OnClick() => button.onClick.AsObservable();
}
