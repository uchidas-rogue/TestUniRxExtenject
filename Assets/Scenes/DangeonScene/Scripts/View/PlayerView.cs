using DG.Tweening;
using UnityEngine;

public class PlayerView : MovingObjectBase
{
    public Vector3 Position { get { return base._transformCash.position; } }

    protected override void Awake ()
    {
        base.Awake ();
    }

    public void SetPosition (Vector3 pos)
    {
        base._transformCash.position = pos;
    }

    int _stateId = Animator.StringToHash("State");
    public void SetAnimation(int State)
    {
        _animator.SetInteger (_stateId, State);
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