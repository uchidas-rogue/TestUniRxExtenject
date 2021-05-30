using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx.Triggers;
using System;
using UniRx;

public class MiniMapView : MonoBehaviour
{
    [SerializeField]
    private RectTransform minimapRect;
    [SerializeField]
    private TextMeshProUGUI maptextGUI;
    [SerializeField]
    private ObservableEventTrigger eventTrigger;

    public void SetMiniMapText (string minimapstring)
    {
        maptextGUI.text = minimapstring;
    }

    public void ChangeMapSize (Vector3 mappos, Vector2 mapsize, float fontSize)
    {
        minimapRect.localPosition = mappos;
        minimapRect.sizeDelta = mapsize;
        maptextGUI.fontSize = fontSize;
        maptextGUI.alignment = TextAlignmentOptions.TopLeft;
        maptextGUI.lineSpacing = -66.5f;
        maptextGUI.characterSpacing = -11.6f;
    }

    public IObservable<UnityEngine.EventSystems.PointerEventData> OnClick()=>eventTrigger.OnPointerClickAsObservable();

}