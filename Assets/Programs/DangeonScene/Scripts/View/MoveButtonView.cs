using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class MoveButtonView : MonoBehaviour
{
    [SerializeField]
    public Button button;
    [SerializeField]
    public float[] vectorNum ;

    public IObservable<Unit> button_OnClick() => button.onClick.AsObservable();
}
