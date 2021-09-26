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
    [SerializeField]
    public GameObject[] EventTiles;
    
    [SerializeField]
    public GameObject[] Items;

    public void SetTile (GameObject tile, int x, int y, float z = 0)
    {
        // Clone the Tiles
        GameObject instance = Instantiate (
            tile,
            new Vector3 (x, z, y),
            Quaternion.identity,
            transform) as GameObject;
        // Change size 
        instance.transform.localScale = new Vector3 (1f, 1f, 1f);
        //instance.transform.localRotation = Quaternion.Euler(90f,0,0);
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