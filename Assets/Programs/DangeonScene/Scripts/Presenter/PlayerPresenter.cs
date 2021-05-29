using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField]
    PlayerView _playerview;
    [SerializeField]
    MoveButtonView[] _moveButtonView;

    IPlayerModel _playermodel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IPlayerModel inject)
    {
        _playermodel = inject;
    }

    void Awake ()
    {
        // unirxでのupdateみたいなやつ => everyupdate
        // keyboard up down left right 監視する
        Observable.EveryUpdate ()
            .Where (_ => !_playermodel.IsPlayerMovingRP.Value && (
                Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow) ||
                Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)
            ))
            .ThrottleFirst (System.TimeSpan.FromSeconds (0.3f)) // 実行間隔の指定
            .Subscribe (_ =>
            {
                _playermodel.ChangeVec3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
            });

        // button onclick register
        foreach (var _moveBtn in _moveButtonView)
        {
            _moveBtn.button_OnClick ()
                .Where (_ => !_playermodel.IsPlayerMovingRP.Value)
                .Subscribe (_ =>
                {
                    _playermodel.ChangeVec3 (_moveBtn.vectorX, _moveBtn.vectorY);
                });
        }

        // Vec3MoveValueRPの変更によって呼び出すように登録する
        _playermodel.Vec3PlayerPositionRP
            .Subscribe (
                dvec3 => _playerview.Move (dvec3)
            );

        _playermodel.DirectionPlayerRP
            .Subscribe (
                dir => _playerview.ChangeSprite (dir)
            );
    }
}