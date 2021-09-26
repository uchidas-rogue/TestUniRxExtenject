using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraView : MonoBehaviour
{
    public Vector3 Offset1 { get; } = new Vector3 (0f, 4f, -4f);
    public Vector3 Euler1 { get; } = new Vector3 (40f, 0f, 0f);
    public Vector3 Offset2 { get; } = new Vector3 (0f, 6f, 0f);
    public Vector3 Euler2 { get; } = new Vector3 (90f, 0f, 0f);

    public Vector3 OffsetPosition { get => _offsetPosition; }
    Transform _transformCash;
    Vector3 _offsetPosition;

    void Awake ()
    {
        _transformCash = GetComponent<Transform> ();
        _offsetPosition = Offset1;
    }

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