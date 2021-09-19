using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapView : MonoBehaviour
{
    RectTransform _miniMapRect;
    TextMeshProUGUI _mapTextGUI;
    ObservableEventTrigger _eventTrigger;

    void Awake ()
    {
        _miniMapRect = GetComponent<RectTransform> ();
        _mapTextGUI = GetComponentInChildren<TextMeshProUGUI> ();
        _eventTrigger = GetComponent<ObservableEventTrigger> ();
    }

    public void SetMiniMapText (string minimapstring) => _mapTextGUI.text = minimapstring;

    public void ChangeMapSize (Vector3 mappos, Vector2 mapsize, bool isAutoSizing = true)
    {
        _miniMapRect.localPosition = mappos;
        _miniMapRect.sizeDelta = mapsize;
        _mapTextGUI.enableAutoSizing = isAutoSizing;
        // autosizingで設定されたフォントサイズをリセット
        if (!isAutoSizing) { _mapTextGUI.fontSize = 50; }
        _mapTextGUI.enableWordWrapping = false;
        _mapTextGUI.lineSpacing = -66.5f;
        _mapTextGUI.characterSpacing = -11.6f;
    }

    public IObservable<PointerEventData> OnClick () => _eventTrigger.OnPointerClickAsObservable ();

}