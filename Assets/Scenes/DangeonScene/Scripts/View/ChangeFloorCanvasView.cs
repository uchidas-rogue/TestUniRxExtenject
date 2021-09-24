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
        var position = transform.position;
        var rotation = transform.rotation;

        Instantiate (
            FloorNumText,
            position,
            rotation,
            transform);

        position.z--;

        Instantiate (
            BlackBack,
            position,
            rotation,
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