using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace TestUniRxExtenject.Assets.Programs.DangeonScene.Scripts.Services
{
    public class MovingObjectService : MonoBehaviour
    {
        public bool IsObjectMoving { get; set; } = false;

        [SerializeField]
        public LayerMask BlockingLayer;
        [SerializeField]
        private float MoveTime = 0.2f;
        [SerializeField]
        private BoxCollider2D boxCollider;
        [SerializeField]
        private Rigidbody2D rb2d;
        [SerializeField]
        protected Transform transformCash;
        private Vector2 tmpVec2 = new Vector2 (0, 0);

        private void ActualMove (Vector3 vector3)
        {
            var endvec3 = (transformCash.position + vector3);
            if (endvec3 != Vector3.zero)
            {
                IsObjectMoving = true;
                transformCash
                    .DOMove (endvec3, MoveTime)
                    .OnComplete (() => IsObjectMoving = false);
            }
        }

        public void AttemptMove (Vector3 vector3)
        {
            RaycastHit2D hit;
            boxCollider.enabled = false;

            hit = Physics2D.Linecast (
                (Vector2) transformCash.position,
                ((Vector2) transformCash.position + (Vector2) vector3),
                BlockingLayer);

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

            boxCollider.enabled = true;

            if (hit.transform == null)
            {
                ActualMove (vector3);
            }
        }

        #region spritechange

        [SerializeField]
        public Sprite[] CharaSprite;
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
}