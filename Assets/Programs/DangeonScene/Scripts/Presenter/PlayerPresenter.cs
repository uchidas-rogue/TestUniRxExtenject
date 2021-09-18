using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    #region injection
    IPlayerModel _playerModel;
    IDangeonFieldModel _dangeonFieldModel;

    IMiniMapStringService _miniMapStringSevice;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IPlayerModel injectpm, IDangeonFieldModel injectdfm, IMiniMapStringService injectmmss)
    {
        _playerModel = injectpm;
        _dangeonFieldModel = injectdfm;
        _miniMapStringSevice = injectmmss;
    }
    #endregion

    [SerializeField]
    PlayerView _playerView;
    [SerializeField]
    MiniMapView _miniMapView;
    [SerializeField]
    MoveButtonView[] _moveButtonView;

    void Awake ()
    {
        _playerView.InitPosition ();
        _playerModel.InitInputVec ();

        // button onclick register
        foreach (var _moveBtn in _moveButtonView)
        {
            // _moveBtn.movebutton_OnUp ()
            //     .Where (_ => !_playerview.IsObjectMoving && !_dangeonFieldModel.IsFieldSetting.Value)
            //     .Subscribe (_ =>
            //     {
            //         _playerModel.ChangeVec3 (_moveBtn.vectorX, _moveBtn.vectorY);
            //     });

            _moveBtn.movebutton_OnDown ()
                .SelectMany (_moveBtn.UpdateAsObservable ())
                .Where (_ => !_playerView.IsObjectMoving && !_dangeonFieldModel.IsFieldSetting)
                // .DoOnCompleted (() =>
                // {
                //     Debug.Log ("released!");
                // })
                .TakeUntil (_moveBtn.movebutton_OnUp ())
                .RepeatUntilDestroy (_moveBtn.gameObject)
                .Subscribe (_ =>
                {
                    //Debug.Log ("press!");
                    _playerModel.ChangeVec3 (_moveBtn.vectorX, _moveBtn.vectorY);
                });
        }

        // PlayerInputVec3RPの変更によって呼び出すように登録する
        _playerModel.PlayerInputVec3RP
            .Subscribe (
                dvec3 => { _playerView.Move (dvec3); }
            );

        // 移動時のキャラ絵の変更
        _playerModel.DirectionPlayerRP
            .Subscribe (
                dir => _playerView.ChangeSprite (dir)
            );

        // playerの位置が変わった時の処理
        _playerModel.PlayerPositionVec3RP
            .Where (ppos => ppos != Vector3.zero && !_dangeonFieldModel.IsFieldSetting)
            .Subscribe (
                ppos =>
                {
                    // if (_dangeonFieldModel.Field[(int) ppos.x, (int) ppos.y, 0] == (int) FieldClass.floor)
                    // {
                    //     //todo
                    //     _playerview.CreateFovFloor (ppos);
                    // }

                    StartCheckWalkedTiles ((int) ppos.x, (int) ppos.y);
                    _miniMapView.SetMiniMapText (
                        _miniMapStringSevice.MakeMiniMapString (
                            (int) ppos.x, (int) ppos.y, _dangeonFieldModel.Field
                        ));
                }
            );

        // player postion get
        var tmpvec3 = new Vector3 ();
        _playerView.UpdateAsObservable ()
            .Subscribe (_ =>
            {
                tmpvec3.Set (Mathf.Ceil (_playerView.transform.position.x), Mathf.Ceil (_playerView.transform.position.y), 0);
                _playerModel.PlayerPositionVec3RP.Value = tmpvec3;
            });

        // unirxでの衝突時の処理の登録 unirx.triggersをusingする
        _playerView.OnTriggerEnter2DAsObservable ()
            .Select (collision => collision.tag)
            .Where (tag => tag == "Stairs")
            .Subscribe (_ =>
            {
                // 移動を停止する ないとミニマップが誤動作する
                _playerView.KillMoving ();
                _playerView.InitPosition ();
                _playerModel.InitInputVec ();

                // SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
                _dangeonFieldModel.FloorNumRP.Value++;
            });

        // unirxでのupdateみたいなやつ => everyupdate
        // keyboard up down left right 監視する
        // _playerview.UpdateAsObservable ()
        //     .Where (_ => !_playerview.IsObjectMoving && !_dangeonFieldModel.IsFieldSetting.Value &&
        //         (
        //             Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow) ||
        //             Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)
        //         ))
        //     //.ThrottleFirst (System.TimeSpan.FromSeconds (0.3f)) // 実行間隔の指定
        //     .Subscribe (_ =>
        //     {
        //         _playerModel.ChangeVec3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
        //     });

        // var upkeyDownStream = this.UpdateAsObservable ().Where (_ => Input.GetKeyDown (KeyCode.UpArrow));
        // var upkeyUpStream = this.UpdateAsObservable ().Where (_ => Input.GetKeyUp (KeyCode.UpArrow));
        // //長押しの判定
        // upkeyDownStream
        //     .Where (_ => !_playerview.IsObjectMoving && !_dangeonFieldModel.IsFieldSetting.Value)
        //     .SelectMany (_ => Observable.Timer(System.TimeSpan.FromSeconds(1)))
        //     //途中でMouseUpされたらストリームをリセット
        //     .TakeUntil (upkeyUpStream)
        //     .RepeatUntilDestroy (this)
        //     .Subscribe (_ =>
        //     {
        //         Debug.Log("press");
        //         _playerModel.ChangeVec3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
        //     });
    }

    /// <summary>
    /// 歩いた場所かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CheckWalkedTile (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y, 1] == 0)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonFieldModel.Field[x, y, 1] = 1;
            if (_dangeonFieldModel.Field[x, y, 0] == 2)
            { // まだフロア内なら
                // さらに周りを調べに行く
                StartCheckWalkedTiles (x, y);
            }
        }
    }

    public void StartCheckWalkedTiles (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y, 0] == 2)
        { // player in floor
            // 八方向全てチェックしに行く
            CheckWalkedTile (x - 1, y - 1);
            CheckWalkedTile (x - 1, y);
            CheckWalkedTile (x - 1, y + 1);
            CheckWalkedTile (x, y - 1);
            CheckWalkedTile (x, y + 1);
            CheckWalkedTile (x + 1, y - 1);
            CheckWalkedTile (x + 1, y);
            CheckWalkedTile (x + 1, y + 1);
        }
        else
        { // これないとフロアに入る前にフロアがマップにでる
            _dangeonFieldModel.Field[x - 1, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x - 1, y, 1] = 1;
            _dangeonFieldModel.Field[x - 1, y + 1, 1] = 1;
            _dangeonFieldModel.Field[x, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x, y + 1, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y + 1, 1] = 1;
        }

    }

}