using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moving object abstract
/// </summary>
public abstract class MovingObject : MonoBehaviour
{
    [SerializeField]
    public LayerMask BlockingLayer;
    [SerializeField]
    public float MoveTime = 0.05f;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    protected Transform transformCash;
    [SerializeField]
    public Sprite[] CharaSprite;
    private Vector2 tmpVec2 = new Vector2 (0, 0);
    private float sqrRemainingDistance = 0;

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        this.sqrRemainingDistance = (transformCash.position - end).sqrMagnitude;

        while (float.Epsilon < this.sqrRemainingDistance)
        {
            this.rb2d.MovePosition (
                Vector3.MoveTowards (this.rb2d.position, end, (1f / this.MoveTime) * Time.deltaTime)
            );
            this.sqrRemainingDistance = (transformCash.position - end).sqrMagnitude;
            yield return null;
        }
    }

    private Vector2 GetTmpVec2 (int xDir, int yDir)
    {
        this.tmpVec2.Set (xDir, yDir);
        return this.tmpVec2;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        //移動先に何らかの物体があるかどうかチェックする。
        //何もない場合はSmoothMovementを呼んで移動する。
        //移動した場合はtrueを返す。
        //Vector2 start = transform.position;
        //Vector2 end = (start + new Vector2 (xDir, yDir));

        this.boxCollider.enabled = false;

        hit = Physics2D.Linecast (
            (Vector2) transformCash.position,
            ((Vector2) transformCash.position + GetTmpVec2 (xDir, yDir)),
            this.BlockingLayer);

        // 斜めの壁抜け防止
        // 斜め方向の移動の場合
        if (hit.transform == null && xDir != 0 && yDir != 0)
        {
            // check x dir
            hit = Physics2D.Linecast (
                (Vector2) transformCash.position,
                ((Vector2) transformCash.position + GetTmpVec2 (xDir, 0)),
                this.BlockingLayer);

            // if xdir null
            if (hit.transform == null)
            {
                // check y dir
                hit = Physics2D.Linecast (
                    (Vector2) transformCash.position,
                    ((Vector2) transformCash.position + GetTmpVec2 (0, yDir)),
                    this.BlockingLayer);
            }
        }

        this.boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine (
                SmoothMovement ((Vector2) transformCash.position + GetTmpVec2 (xDir, yDir))
            );
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove (int xDir, int yDir)
    {
        //MoveやOnCantMoveといった移動処理に関する一連の処理を呼び出す。
        //外部のクラスからこのオブジェクトを移動させるための入り口。
        RaycastHit2D raycasthit2d;
        Move (xDir, yDir, out raycasthit2d);
    }

    #region spritechange

    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    public void ChangeSprite (Direction dir)
    {
        // none なら何もしない
        if (dir == Direction.none) { return; }
        if ((int) dir > 0)
        {
            spriteRenderer.sprite = CharaSprite[(int) dir - 1];
        }
        else
        {
            spriteRenderer.sprite = CharaSprite[Mathf.Abs ((int) dir) + 3];
        }

    }

    #endregion

}