using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void ChangeMapSize (Vector3 mappos, Vector2 mapsize, bool IsAutoSizing = true)
    {
        minimapRect.localPosition = mappos;
        minimapRect.sizeDelta = mapsize;
        maptextGUI.enableAutoSizing = IsAutoSizing;
        // autosizingで設定されたフォントサイズをリセット
        if (!IsAutoSizing) { maptextGUI.fontSize = 50; }
        maptextGUI.enableWordWrapping = false;
        maptextGUI.lineSpacing = -66.5f;
        maptextGUI.characterSpacing = -11.6f;

    }

    public IObservable<PointerEventData> OnClick () => eventTrigger.OnPointerClickAsObservable ();

}