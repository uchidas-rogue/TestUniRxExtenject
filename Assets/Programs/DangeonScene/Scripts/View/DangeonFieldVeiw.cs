using UnityEngine;

public class DangeonFieldVeiw : MonoBehaviour
{
    //floor prefab 
    [SerializeField]
    public GameObject[] FloorTiles;
    //wall prefab
    [SerializeField]
    public GameObject[] WallTiles;
    [SerializeField]
    public GameObject StairsTile;
    public void SetTile (GameObject tile, int x, int y)
    {
        // Clone the Tiles
        GameObject instance = Instantiate (
            tile,
            new Vector3 (x, y, 0),
            Quaternion.identity,
            transform) as GameObject;
        // Change size 
        instance.transform.localScale = new Vector3 (1.05f, 1.05f, 0f);
    }

    public void RemoveAllTiles ()
    {
        // Remove all child object 
        foreach (Transform item in transform)
        {
            GameObject.Destroy (item.gameObject);
        }
    }
}