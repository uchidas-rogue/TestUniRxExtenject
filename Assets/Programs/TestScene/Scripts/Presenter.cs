using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class Presenter : MonoBehaviour
{
    [SerializeField]
    View _view;
    IModel _model;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor(IModel inject)
    {
        _model = inject;
    }

    void Awake()
    {
        // unirxでのupdateみたいなやつ
        Observable.EveryUpdate()
            .Where(xs => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
            .Subscribe(_=>_model.ChangeVec3(new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"))));
        // Vec3MoveValueRPの変更によってviewのMoveを呼び出すように登録する
        _model.Vec3MoveValueRP.Subscribe(dvec3=>_view.Move(dvec3));
    
    }
}
