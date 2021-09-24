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

    int cnt = 0;

    public void SetTile (GameObject tile, int x, int y, int z = 0)
    {
        for (cnt = 0; cnt <= z; cnt++)
        {
            // Clone the Tiles
            GameObject instance = Instantiate (
                tile,
                new Vector3 (x, cnt, y),
                Quaternion.identity,
                transform) as GameObject;
            // Change size 
            instance.transform.localScale = new Vector3 (1f, 1f, 1f);
        }
        // // Clone the Tiles
        // GameObject instance = Instantiate (
        //     tile,
        //     new Vector3 (x, z, y),
        //     Quaternion.identity,
        //     transform) as GameObject;
        // // Change size 
        // instance.transform.localScale = new Vector3 (1f, 1f, 1f);
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