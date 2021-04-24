using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DangeonFieldVeiw : MonoBehaviour
{
    public void SetTile (GameObject tile, int x, int y)
    {
        // Clone the Tiles
        GameObject instance = Instantiate (
            tile,
            new Vector3 (x, y, 0),
            Quaternion.identity,
            transform) as GameObject;
        // Change size 
        instance.transform.localScale = new Vector3(1.04f,1.04f,0f);
    }

    public void RemoveAllTiles()
    {
        // Remove all child object 
        foreach (Transform item in transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}