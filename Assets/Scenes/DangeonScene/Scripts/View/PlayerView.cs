using DG.Tweening;
using UnityEngine;

public class PlayerView : MovingObjectBase
{
    Vector3 _initPosVec3 = new Vector3 (49f, 0, 49f);

    public Vector3 Position { get { return base._transformCash.position; } }

    protected override void Awake ()
    {
        base.Awake ();
    }

    public void InitPosition ()
    {
        base._transformCash.position = _initPosVec3;
    }

    public void Move (Vector3 inputVec3) => base.AttemptMove (inputVec3);

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