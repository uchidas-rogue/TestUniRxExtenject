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
    IMoveObjectServece _moveObjectSevice;
    IPlayerModel _playerModel;
    IDangeonFieldModel _dangeonFieldModel;
    IMiniMapStringService _miniMapStringSevice;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IMoveObjectServece injectmos, IPlayerModel injectpm,
        IDangeonFieldModel injectdfm, IMiniMapStringService injectmmss)
    {
        _moveObjectSevice = injectmos;
        _playerModel = injectpm;
        _dangeonFieldModel = injectdfm;
        _miniMapStringSevice = injectmmss;
    }
    #endregion // injection

    #region views
    PlayerView _playerView;
    MiniMapView _miniMapView;
    MainCameraView _mainCameraVeiw;
    MoveButtonView[] _moveButtonView;
    MoveCameraButtonView _moveCameraButtonView;

    void Awake ()
    {
        _playerView = GetComponent<PlayerView> ();
        _miniMapView = FindObjectOfType<MiniMapView> ();
        _moveButtonView = FindObjectsOfType<MoveButtonView> ();
        _mainCameraVeiw = FindObjectOfType<MainCameraView> ();
        _moveCameraButtonView = FindObjectOfType<MoveCameraButtonView> ();
    }
    #endregion // views

    void Start ()
    {
        PlayerInit ();

        _moveCameraButtonView.OnClick ()
            .Subscribe (_ =>
            {
                if (_mainCameraVeiw.OffsetPosition == _mainCameraVeiw._offset1)
                {
                    _mainCameraVeiw.Rotation (_mainCameraVeiw._euler2, _mainCameraVeiw._offset2);
                }
                else
                {
                    _mainCameraVeiw.Rotation (_mainCameraVeiw._euler1, _mainCameraVeiw._offset1);
                }
            });

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
                .Subscribe (_ => SetPlayerInputVec (_moveBtn.VectorX, _moveBtn.VectorY));
        }

        // PlayerInputVec3RPの変更によって呼び出すように登録する
        _playerModel.PlayerInputVec3RP
            .Subscribe (
                dvec3 => { _playerView.Move (dvec3); }
            );

        // 移動時のキャラ絵の変更
        _playerModel.DirectionPlayerRP
            .Subscribe (
                dir => _playerView.ChangeDirection (dir)
            );

        // playerの位置が変わった時の処理
        _playerModel.PlayerPositionVec3RP
            .Where (ppos => ppos != Vector3.zero) //&& !_dangeonFieldModel.IsFieldSettingRP.Value)
            .Subscribe (
                ppos =>
                {
                    // if (_dangeonFieldModel.Field[(int) ppos.x, (int) ppos.y] == (int) FieldClass.floor)
                    // {
                    //     //todo
                    //     _playerview.CreateFovFloor (ppos);
                    // }

                    StartCheckWalkedTiles ((int) ppos.x, (int) ppos.z);
                    _miniMapView.SetMiniMapText (
                        _miniMapStringSevice.MakeMiniMapString (
                            (int) ppos.x, (int) ppos.z
                        ));
                }
            );

        // player postion get
        var tmpvec3 = new Vector3 ();
        _playerView.UpdateAsObservable ()
            //.Where(_ => !_playerView.IsObjectMoving)
            .Subscribe (_ =>
            {
                _mainCameraVeiw.Move (_playerView.GetPlayerPosition ());
                // 移動中はプレイヤー位置の設定を行わない
                if (_playerView.IsObjectMoving) { return; }

                tmpvec3.Set (
                    _playerView.GetPlayerPosition ().x,
                    0,
                    _playerView.GetPlayerPosition ().z
                );
                _playerModel.PlayerPositionVec3RP.Value = tmpvec3;
            });

        // unirxでの衝突時の処理の登録 unirx.triggersをusingする
        _playerView.OnTriggerStayAsObservable ()
            .Select (collision => collision.tag)
            .Where (_ => !_playerView.IsObjectMoving)
            .Subscribe (tag =>
            {
                switch (tag)
                {
                    case "Stairs":
                        PlayerInit ();
                        _dangeonFieldModel.FloorNumRP.Value++;
                        break;
                }
            });
        // reload scene
        // SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);

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
    /// プレイヤーの移動入力と位置の初期化
    /// </summary>
    void PlayerInit ()
    {
        _playerView.InitPosition ();
        _playerModel.PlayerInputVec3RP.Value = Vector3.zero;
    }

    /// <summary>
    /// プレイヤーの移動入力の設定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetPlayerInputVec (float x, float y)
    {
        _playerModel.PlayerInputVec3RP.Value = Vector3.zero;
        _playerModel.DirectionPlayerRP.Value = _moveObjectSevice.GetInputDirection (x, y);
        _playerModel.PlayerInputVec3RP.Value = _moveObjectSevice.GetInputVec (x, y);
    }

    /// <summary>
    /// 歩いた場所かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CheckWalkedTile (int x, int y)
    {
        if (_dangeonFieldModel.Map[x, y] != MapClass.walked)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonFieldModel.Map[x, y] = MapClass.walked;
            if (_dangeonFieldModel.Field[x, y] == FieldClass.floor)
            { // まだフロア内なら
                // さらに周りを調べに行く
                StartCheckWalkedTiles (x, y);
            }
        }
    }

    void StartCheckWalkedTiles (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y] == FieldClass.floor)
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
            _dangeonFieldModel.Map[x - 1, y - 1] = MapClass.walked;
            _dangeonFieldModel.Map[x - 1, y] = MapClass.walked;
            _dangeonFieldModel.Map[x - 1, y + 1] = MapClass.walked;
            _dangeonFieldModel.Map[x, y - 1] = MapClass.walked;
            _dangeonFieldModel.Map[x, y + 1] = MapClass.walked;
            _dangeonFieldModel.Map[x + 1, y - 1] = MapClass.walked;
            _dangeonFieldModel.Map[x + 1, y] = MapClass.walked;
            _dangeonFieldModel.Map[x + 1, y + 1] = MapClass.walked;
        }

    }

}