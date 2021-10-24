using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapView : MonoBehaviour
{
    public Vector3 PickedMapPositionVec3 { get; } = new Vector3 (0f, 0f, 0f);
    public Vector2 PickedMapSizeVec2 { get; } = new Vector2 (1920f, 1080f);
    public Vector3 MapPositionVec3 { get; } = new Vector3 (-665f, 315f, 0f);
    public Vector2 MapSizeVec2 { get; } = new Vector2 (520f, 380f);

    public Vector2 MapSize { get { return _mapRect.sizeDelta; } }
    RectTransform _mapRect;
    TextMeshProUGUI _mapTextGUI;
    ObservableEventTrigger _eventTrigger;

    void Awake ()
    {
        _mapRect = GetComponent<RectTransform> ();
        _mapTextGUI = GetComponentInChildren<TextMeshProUGUI> ();
        _eventTrigger = GetComponent<ObservableEventTrigger> ();
    }

    public void SetMapText (string minimapstring) => _mapTextGUI.text = minimapstring;

    public void ChangeMapSize (Vector3 mappos, Vector2 mapsize, bool isAutoSizing = true)
    {
        _mapRect.localPosition = mappos;
        _mapRect.sizeDelta = mapsize;
        _mapTextGUI.enableAutoSizing = isAutoSizing;
        // autosizingで設定されたフォントサイズをリセット
        if (!isAutoSizing) { _mapTextGUI.fontSize = 50; }
        _mapTextGUI.enableWordWrapping = false;
        _mapTextGUI.lineSpacing = -66.5f;
        _mapTextGUI.characterSpacing = -11.6f;
    }

    public IObservable<PointerEventData> OnClick () => _eventTrigger.OnPointerClickAsObservable ();

}