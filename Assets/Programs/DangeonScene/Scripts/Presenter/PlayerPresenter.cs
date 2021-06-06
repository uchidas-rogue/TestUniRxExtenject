using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    #region injection
    IPlayerModel _playerModel;
    IDangeonFieldModel _dangeonFieldModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IPlayerModel injectpm, IDangeonFieldModel injectdfm)
    {
        _playerModel = injectpm;
        _dangeonFieldModel = injectdfm;
    }
    #endregion

    [SerializeField]
    PlayerView _playerview;
    [SerializeField]
    MoveButtonView[] _moveButtonView;

    void Awake ()
    {
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
                .SelectMany (_ => _moveBtn.UpdateAsObservable ())
                .Where (_ => !_playerview.IsObjectMoving && !_dangeonFieldModel.IsFieldSetting.Value)
                .TakeUntil (_moveBtn.movebutton_OnUp ())
                // .DoOnCompleted (() =>
                // {
                //     Debug.Log ("released!");
                // })
                .RepeatUntilDestroy (_moveBtn)
                .Subscribe (_ =>
                {
                    //Debug.Log ("press!");
                    _playerModel.ChangeVec3 (_moveBtn.vectorX, _moveBtn.vectorY);
                });
        }

        // PlayerInputVec3RPの変更によって呼び出すように登録する
        _playerModel.PlayerInputVec3RP
            .Subscribe (
                dvec3 => _playerview.Move (dvec3)
            );
        // 移動時のキャラ絵の変更
        _playerModel.DirectionPlayerRP
            .Subscribe (
                dir => _playerview.ChangeSprite (dir)
            );

        // playerの位置が変わった時の処理
        // _playerModel.PlayerPositionVec3RP.Subscribe (Pos => Debug.Log (Pos.x + "," + Pos.y));

        // フロア変わったら初期位置に移動
        _dangeonFieldModel.FloorNumRP
            .Subscribe (_ =>
            {
                _playerview.InitPosition ();
                _playerModel.InitInputVec ();
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

        // player postion get
        var tmpvec3 = new Vector3 ();
        _playerview.UpdateAsObservable ()
            .Subscribe (
                _ =>
                {
                    tmpvec3.Set (Mathf.Ceil (_playerview.transform.position.x), Mathf.Ceil (_playerview.transform.position.y), 0);
                    _playerModel.PlayerPositionVec3RP.Value = tmpvec3;
                });

        // unirxでの衝突時の処理の登録 unirx.triggersをusingする
        _playerview.OnTriggerEnter2DAsObservable ()
            .Select (collision => collision.tag)
            .Where (tag => tag == "Stairs")
            .Subscribe (_ =>
            {
                // SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
                _dangeonFieldModel.FloorNumRP.Value++;
            });
    }
}