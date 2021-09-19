using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFloorCanvasView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI FloorNumtextGUI;
    public void SetFloorNumText (string floorNumString) => FloorNumtextGUI.text = floorNumString;

    public void SetActiveAll (bool isActive)
    {
        // Remove all child object 
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(isActive);
        }
    }
}