using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapView : MonoBehaviour
{
    [SerializeField]
    private RectTransform MiniMapRect;
    [SerializeField]
    private TextMeshProUGUI MapTextGUI;
    [SerializeField]
    private ObservableEventTrigger EventTrigger;

    public void SetMiniMapText (string minimapstring) => MapTextGUI.text = minimapstring;

    public void ChangeMapSize (Vector3 mappos, Vector2 mapsize, bool isAutoSizing = true)
    {
        MiniMapRect.localPosition = mappos;
        MiniMapRect.sizeDelta = mapsize;
        MapTextGUI.enableAutoSizing = isAutoSizing;
        // autosizingで設定されたフォントサイズをリセット
        if (!isAutoSizing) { MapTextGUI.fontSize = 50; }
        MapTextGUI.enableWordWrapping = false;
        MapTextGUI.lineSpacing = -66.5f;
        MapTextGUI.characterSpacing = -11.6f;
    }

    public IObservable<PointerEventData> OnClick () => EventTrigger.OnPointerClickAsObservable ();

}