using DG.Tweening;
using UnityEngine;

public class PlayerView : MovingObjectBase
{
    Vector3 _initPosVec3 = new Vector3 (49f, 0, 49f);
    Vector3 _mainCameraOffset;
    Transform _mainCamera;

    protected override void Awake ()
    {
        base.Awake ();
        _mainCamera = Camera.main.transform;
        _mainCameraOffset = _mainCamera.position;
    }

    public void InitPosition ()
    {
        base._transformCash.position = _initPosVec3;
    }

    public void Move (Vector3 inputVec3) => base.AttemptMove (inputVec3);

    public void MainCameraTrackMove()
    {
        _mainCamera.position = _mainCameraOffset + _transformCash.position;
    }

    public Vector3 GetPlayerPosition () => base._transformCash.position;

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