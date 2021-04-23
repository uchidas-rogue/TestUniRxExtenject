using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public void Move(Vector3 vector3)
    {
        transform.position += vector3;
    }
}
