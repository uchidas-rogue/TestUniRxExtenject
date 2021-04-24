using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField]
    PlayerView _playerview;

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
            .Where(_ => _playermodel.CheckKeyInput(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")))
            .Subscribe(_=>_playermodel.ChangeVec3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));

        // Vec3MoveValueRPの変更によってviewのMoveを呼び出すように登録する
        _playermodel.Vec3PlayerPositionRP.Subscribe(dvec3=>_playerview.Move(dvec3));
    
    }
}
