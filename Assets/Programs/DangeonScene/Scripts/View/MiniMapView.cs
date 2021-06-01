using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx.Triggers;

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
        maptextGUI.alignment = TextAlignmentOptions.MidlineJustified;
        maptextGUI.lineSpacing = -66.5f;
        maptextGUI.characterSpacing = -11.6f;
    }

    public IObservable<PointerEventData> OnClick()=>eventTrigger.OnPointerClickAsObservable();

}