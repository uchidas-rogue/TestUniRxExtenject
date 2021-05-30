using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField]
    PlayerView _playerview;
    [SerializeField]
    MoveButtonView[] _moveButtonView;

    IPlayerModel _playermodel;
    IDangeonFieldModel _dangeonfieldmodel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IPlayerModel injectpm, IDangeonFieldModel injectdfm)
    {
        _playermodel = injectpm;
        _dangeonfieldmodel = injectdfm;
    }

    void Awake ()
    {
        // button onclick register
        foreach (var _moveBtn in _moveButtonView)
        {
            _moveBtn.movebutton_OnClick ()
                .Where (_ => !_playermodel.IsPlayerMovingRP.Value)
                .Subscribe (_ =>
                {
                    _playermodel.ChangeVec3 (_moveBtn.vectorX, _moveBtn.vectorY);
                });
        }

        // PlayerInputVec3RPの変更によって呼び出すように登録する
        _playermodel.PlayerInputVec3RP
            .Subscribe (
                dvec3 => _playerview.Move (dvec3)
            );
        // 移動時のキャラ絵の変更
        _playermodel.DirectionPlayerRP
            .Subscribe (
                dir => _playerview.ChangeSprite (dir)
            );

        // playerの位置が変わった時の処理
        _playermodel.PlayerPositionVec3RP.Subscribe (Pos => Debug.Log (Pos.x + "," + Pos.y));

        // unirxでのupdateみたいなやつ => everyupdate
        // keyboard up down left right 監視する
        _playerview.UpdateAsObservable ()
            .Where (_ => !_playermodel.IsPlayerMovingRP.Value && (
                Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow) ||
                Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)
            ))
            .ThrottleFirst (System.TimeSpan.FromSeconds (0.3f)) // 実行間隔の指定
            .Subscribe (_ =>
            {
                _playermodel.ChangeVec3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
            });

        // player postion get
        var tmpvec3 = new Vector3();
        _playerview.UpdateAsObservable()
            .Subscribe (
                _ => {
                tmpvec3.Set(Mathf.Ceil(_playerview.transform.position.x),Mathf.Ceil(_playerview.transform.position.y),0);
                _playermodel.PlayerPositionVec3RP.Value = tmpvec3;
            });

        // unirxでの衝突時の処理の登録 unirx.triggersをusingする
        _playerview.OnTriggerEnter2DAsObservable ()
            .Select (collision => collision.tag)
            .Where (tag => tag == "Stairs")
            .Subscribe (_ =>
            {
                // SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
                _playermodel.IsPlayerMovingRP.Value = true;
                _playermodel.InitInputVec();
                _playerview.InitPosition();
                _dangeonfieldmodel.FloorNumRP.Value++;
                _playermodel.IsPlayerMovingRP.Value = false;
            });
    }
}