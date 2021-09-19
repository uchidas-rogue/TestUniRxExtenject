using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFloorCanvasView : MonoBehaviour
{
    [SerializeField]
    GameObject FloorNumText;
    [SerializeField]
    GameObject BlackBack;

    TextMeshProUGUI _floorNumText;

    void Awake ()
    {
        Instantiate (
            FloorNumText,
            new Vector3 (0, 0, 0),
            Quaternion.identity,
            transform);

        Instantiate (
            BlackBack,
            new Vector3 (0, 0, 0),
            Quaternion.identity,
            transform);

        _floorNumText = GetComponentInChildren<TextMeshProUGUI> ();
    }

    public void SetFloorNumText (string floorNumString) => _floorNumText.text = floorNumString;

    public void SetActiveAll (bool isActive)
    {
        // Remove all child object 
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive (isActive);
        }
    }
}