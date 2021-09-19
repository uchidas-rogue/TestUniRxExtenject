using DG.Tweening;
using UnityEngine;

public class MovingObjectBase : MonoBehaviour
{
    public bool IsObjectMoving { get; set; } = false;

    [SerializeField]
    float MoveTime = 0.2f;
    protected Transform _transformCash;
    LayerMask _blockingLayer;
    BoxCollider2D _boxCollider;
    Rigidbody2D _rb2d;

    void Awake ()
    {
        _transformCash = GetComponent<Transform> ();
        _blockingLayer = LayerMask.GetMask ("Blocking");
        _boxCollider = GetComponent<BoxCollider2D> ();
        _rb2d = GetComponent<Rigidbody2D> ();
        _spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void KillMoving ()
    {
        _transformCash.DOKill ();
    }

    /// <summary>
    /// 実際に移動処理
    /// </summary>
    /// <param name="inpVec3"></param>
    void ActualMove (Vector3 inpVec3)
    {
        var endvec3 = (_transformCash.position + inpVec3);
        if (endvec3 != Vector3.zero)
        {
            IsObjectMoving = true;
            _transformCash
                .DOMove (endvec3, MoveTime)
                .OnComplete (() => IsObjectMoving = false);
        }
    }

    /// <summary>
    /// 入力方向への移動を試す
    /// </summary>
    /// <param name="inpVec3"></param>
    public void AttemptMove (Vector3 inpVec3)
    {
        RaycastHit2D hit;
        _boxCollider.enabled = false;

        hit = Physics2D.Linecast (
            (Vector2) _transformCash.position,
            ((Vector2) _transformCash.position + (Vector2) inpVec3),
            _blockingLayer);

        // 斜めの壁抜け防止
        // 斜め方向の移動の場合
        // if (hit.transform == null && vector3.x != 0 && vector3.y != 0)
        // {
        //     // check x dir
        //     hit = Physics2D.Linecast (
        //         (Vector2) transformCash.position,
        //         ((Vector2) transformCash.position + GetTmpVec2 (xDir, 0)),
        //         this.BlockingLayer);

        //     // if xdir null
        //     if (hit.transform == null)
        //     {
        //         // check y dir
        //         hit = Physics2D.Linecast (
        //             (Vector2) transformCash.position,
        //             ((Vector2) transformCash.position + GetTmpVec2 (0, yDir)),
        //             this.BlockingLayer);
        //     }
        // }

        _boxCollider.enabled = true;

        if (hit.transform == null)
        {
            ActualMove (inpVec3);
        }
    }

    #region spritechange

    [SerializeField]
    protected Sprite[] CharaSprite;
    protected SpriteRenderer _spriteRenderer;

    public void ChangeSprite (Direction dir)
    {
        // none なら何もしない
        if (dir == Direction.none) { return; }
        if ((int) dir > 0)
        {
            _spriteRenderer.sprite = CharaSprite[(int) dir - 1];
        }
        else
        {
            _spriteRenderer.sprite = CharaSprite[Mathf.Abs ((int) dir) + 3];
        }

    }

    #endregion
}