using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFromKeyboard : IInputable
{
    public Vector3 InputForMove() => new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
}
