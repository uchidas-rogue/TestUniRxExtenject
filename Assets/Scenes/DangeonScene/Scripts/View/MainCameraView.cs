using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraView : MonoBehaviour
{
    Transform _transformCash;
    Vector3 _offsetPosition;

    [System.NonSerialized]
    public Vector3 _offset1 = new Vector3 (0f, 4f, -4f);
    [System.NonSerialized]
    public Vector3 _euler1 = new Vector3 (40f, 0f, 0f);
    [System.NonSerialized]
    public Vector3 _offset2 = new Vector3 (0f, 6f, 0f);
    [System.NonSerialized]
    public Vector3 _euler2 = new Vector3 (90f, 0f, 0f);

    void Awake ()
    {
        _transformCash = GetComponent<Transform> ();
        _offsetPosition = _offset1;
    }

    public Vector3 OffsetPosition { get => _offsetPosition; }

    public void Move (Vector3 pos)
    {
        _transformCash.position = _offsetPosition + pos;
    }

    public void Rotation (Vector3 eulerVec, Vector3 offsetVec)
    {
        _transformCash.localRotation = Quaternion.Euler (eulerVec);
        _offsetPosition = offsetVec;
    }
}