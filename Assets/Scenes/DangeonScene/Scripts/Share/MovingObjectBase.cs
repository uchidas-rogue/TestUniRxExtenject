using DG.Tweening;
using UnityEngine;

public class MovingObjectBase : MonoBehaviour
{
    public bool IsObjectMoving { get; set; } = false;

    [SerializeField]
    float MoveTime = 0.2f;
    protected Transform _transformCash;
    LayerMask _blockingLayer;
    BoxCollider _boxCollider;
    Rigidbody _rigidbody;
    protected Animator _animator;

    protected virtual void Awake ()
    {
        _transformCash = GetComponent<Transform> ();
        _blockingLayer = LayerMask.GetMask ("Blocking");
        _boxCollider = GetComponent<BoxCollider> ();
        _rigidbody = GetComponent<Rigidbody> ();
        //_spriteRenderer = GetComponent<SpriteRenderer> ();
        _animator = GetComponent<Animator> ();
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
        if ((_transformCash.position + inpVec3) != Vector3.zero)
        {
            IsObjectMoving = true;
            _animator.SetInteger("State",1);
            _transformCash
                .DOMove (_transformCash.position + inpVec3, MoveTime)
                .OnComplete (() =>
                {
                    IsObjectMoving = false;
                    _animator.SetInteger("State",0);
                });
        }
    }

    /// <summary>
    /// 入力方向への移動を試す
    /// </summary>
    /// <param name="inpVec3"></param>
    public void AttemptMove (Vector3 inpVec3)
    {
        bool ishit;
        _boxCollider.enabled = false;

        ishit = Physics.Linecast (
            _transformCash.position,
            _transformCash.position + inpVec3,
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

        if (!ishit)
        {
            ActualMove (inpVec3);
        }
    }

    public void ChangeDirection (Direction dir)
    {
        //none なら何もしない
        if (dir == Direction.none) { return; }

        switch (dir)
        {
            case Direction.up:
                _transformCash.DOLocalRotate (Vector3.up * 0, 0f);
                break;
            case Direction.upright:
                _transformCash.DOLocalRotate (Vector3.up * 45, 0f);
                break;
            case Direction.right:
                _transformCash.DOLocalRotate (Vector3.up * 90, 0f);
                break;
            case Direction.downright:
                _transformCash.DOLocalRotate (Vector3.up * 135, 0f);
                break;
            case Direction.down:
                _transformCash.DOLocalRotate (Vector3.up * 180, 0f);
                break;
            case Direction.downleft:
                _transformCash.DOLocalRotate (Vector3.up * -135, 0f);
                break;
            case Direction.left:
                _transformCash.DOLocalRotate (Vector3.up * -90, 0f);
                break;
            case Direction.upleft:
                _transformCash.DOLocalRotate (Vector3.up * -45, 0f);
                break;
        }

    }

    // #region spritechange

    // [SerializeField]
    // protected Sprite[] CharaSprite;
    // protected SpriteRenderer _spriteRenderer;

    // public void ChangeSprite (Direction dir)
    // {
    //     // none なら何もしない
    //     if (dir == Direction.none) { return; }
    //     if ((int) dir > 0)
    //     {
    //         _spriteRenderer.sprite = CharaSprite[(int) dir - 1];
    //     }
    //     else
    //     {
    //         _spriteRenderer.sprite = CharaSprite[Mathf.Abs ((int) dir) + 3];
    //     }

    // }

    // #endregion
}