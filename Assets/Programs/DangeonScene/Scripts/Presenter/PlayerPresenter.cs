using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
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
    public void Constructor(IPlayerModel inject)
    {
        _playermodel = inject;
    }

    void Awake()
    {
        // unirxでのupdateみたいなやつ
        Observable.EveryUpdate()
            .Where(_ => !_playermodel.IsPlayerTurnRP.Value && _playermodel.CheckKeyInput())
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.2f)) // 実行間隔の指定
            .Subscribe(_=>
                _playermodel.ChangeVec3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"))
                );

        // Vec3MoveValueRPの変更によってviewのMoveを呼び出すように登録する
        _playermodel.Vec3PlayerPositionRP
            .Subscribe(
                dvec3 =>_playerview.Move(dvec3)
            );

        // button onclick register
        foreach (var _moveBtn in _moveButtonView)
        {
            _moveBtn.button_OnClick()
                .Where(_ => !_playermodel.IsPlayerTurnRP.Value)
                .Subscribe(_=>
                    _playermodel.ChangeVec3(_moveBtn.vectorNum[0],_moveBtn.vectorNum[1])
                );
        }
    }
}
