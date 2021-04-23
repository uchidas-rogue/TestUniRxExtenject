using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputForMove : MonoBehaviour
{

    [Inject]
    private IInputable _inputObject;

    // Start is called before the first frame update
    void Move(Vector3 vec)
    {
        var position = transform.localPosition;
        transform.localPosition = position + vec;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputObject != null)
        {
            Move(_inputObject.InputForMove());
        }
    }
}
