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
    [SerializeField]
    public float[] vectorNum;
    [SerializeField]
    public Sprite CharaSprite;

    public IObservable<Unit> button_OnClick () => button.onClick.AsObservable ();
}