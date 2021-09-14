using DG.Tweening;
using TestUniRxExtenject.Assets.Programs.DangeonScene.Scripts.Services;
using UnityEngine;

public class PlayerView : MovingObjectService
{
    [SerializeField]
    public GameObject FovFloor;

    private Vector3 InitPosVec3 = new Vector3 (49f, 49f, 0);
    public void Move (Vector3 vector3)
    {
        //base.AttemptMove ((int) vector3.x, (int) vector3.y);

        base.AttemptMove(vector3);
    }

    public void InitPosition ()
    {
        base.transformCash.position = InitPosVec3;
        //base.transformCash.DOMove(InitPosVec3,0f);
    }

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