using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class Presenter : MonoBehaviour
{
    [SerializeField]
    View _view;
    [SerializeField]
    ButtonView _buttonView;

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
            .Where(_ => _model.CheckKeyInput(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")))
            .Subscribe(_=>_model.ChangeVec3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));

        // Vec3MoveValueRPの変更によってviewのMoveを呼び出すように登録する
        _model.Vec3MoveValueRP.Subscribe(dvec3=>_view.Move(dvec3));

        // button onclick register
        _buttonView.button_OnClick().Subscribe(_=>_model.ChangeVec3(1f,0f));
    
    }
}
