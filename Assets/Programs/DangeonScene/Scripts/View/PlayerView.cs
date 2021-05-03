using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MovingObject
{
    [SerializeField]
    public Sprite[] CharaSprite;
    public void Move (Vector3 vector3)
    {
        base.AttemptMove ((int) vector3.x, (int) vector3.y);
    }

    public void ChangeSprite (Direction dir)
    {
        // none なら何もしない
        if (dir == Direction.none) { return; }
        if ((int) dir < 0)
        {
            base.spriteRenderer.sprite = CharaSprite[Mathf.Abs ((int) dir) + 3];
        }
        else
        {
            base.spriteRenderer.sprite = CharaSprite[(int) dir - 1];
        }

    }
}