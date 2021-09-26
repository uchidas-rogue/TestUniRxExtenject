using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraView : MonoBehaviour
{
    Transform _transformCash;
    Vector3 _offSetPosition;
    
    Vector3 _offset1 = new Vector3 (0f, 4f, -4f);
    Vector3 _offset2 = new Vector3 (0f, 6f, 0f);

    void Awake ()
    {
        _transformCash = GetComponent<Transform> ();
        _offSetPosition = _offset1;
    }

    public void Move (Vector3 pos)
    {
        _transformCash.position = _offSetPosition + pos;
    }

    public void Rotation ()
    {
        if (_offSetPosition == _offset1)
        {
            _transformCash.localRotation = Quaternion.Euler (90f, 0f, 0f);
            _offSetPosition = _offset2;
        }
        else
        {
            _transformCash.localRotation = Quaternion.Euler (40f, 0f, 0f);
            _offSetPosition = _offset1;
        }

    }
}