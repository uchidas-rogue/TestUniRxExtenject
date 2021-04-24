using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moving object abstract
/// </summary>
public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    public float moveTime = 0.05f;
    private Rigidbody2D rb2d;
    private float inverseMoveTime;
    private Vector2 tmpVec2 = new Vector2 (0, 0);
    private float sqrRemainingDistance = 0;

    protected virtual void Start ()
    {
        this.rb2d = GetComponent<Rigidbody2D> ();
        this.boxCollider = GetComponent<BoxCollider2D> ();
        this.inverseMoveTime = 1f / moveTime;
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (float.Epsilon < sqrRemainingDistance)
        {
            rb2d.MovePosition (Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime));
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    private Vector2 GetTmpVec2 (int xDir, int yDir)
    {
        tmpVec2.Set (xDir, yDir);
        return tmpVec2;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        //移動先に何らかの物体があるかどうかチェックする。
        //何もない場合はSmoothMovementを呼んで移動する。
        //移動した場合はtrueを返す。
        //Vector2 start = transform.position;
        //Vector2 end = (start + new Vector2 (xDir, yDir));

        //this.boxCollider.enabled = false;

        hit = Physics2D.Linecast ((Vector2) transform.position, ((Vector2) transform.position + GetTmpVec2 (xDir, yDir)), this.blockingLayer);
        //斜めの壁抜け防止
        if (hit.transform == null && xDir != 0 && yDir != 0)
        {
            hit = Physics2D.Linecast ((Vector2) transform.position, ((Vector2) transform.position + GetTmpVec2 (xDir, 0)), this.blockingLayer);
            if (hit.transform == null)
            {
                hit = Physics2D.Linecast ((Vector2) transform.position, ((Vector2) transform.position + GetTmpVec2 (0, yDir)), this.blockingLayer);
            }
        }

        //this.boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine (SmoothMovement ((Vector2) transform.position + GetTmpVec2 (xDir, yDir)));
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove (int xDir, int yDir)
    {
        //MoveやOnCantMoveといった移動処理に関する一連の処理を呼び出す。
        //外部のクラスからこのオブジェクトを移動させるための入り口。
        RaycastHit2D hit;
        Move (xDir, yDir, out hit);
    }
}