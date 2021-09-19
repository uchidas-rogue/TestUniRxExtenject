using DG.Tweening;
using UnityEngine;

public class PlayerView : MovingObjectBase
{
    [SerializeField]
    public GameObject FovFloor;

    private Vector3 _initPosVec3 = new Vector3 (49f, 49f, 0);
    public void Move (Vector3 inputVec3) => base.AttemptMove (inputVec3);
    public void InitPosition () => base.TransformCash.position = _initPosVec3;

    public void CreateFovFloor (Vector3 ppos)
    {
        // GameObject instance = Instantiate (
        //     FovFloor,
        //     ppos,
        //     Quaternion.identity,
        //     transform) as GameObject;
        // // Change size 
        // instance.transform.localScale = new Vector3 (5f, 5f, 0f);
    }
}