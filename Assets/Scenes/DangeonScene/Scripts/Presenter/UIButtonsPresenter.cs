using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class UIButtonsPresenter : MonoBehaviour
{
    #region injection

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor ()
    {

    }
    #endregion

    #region viewscript
    AttackButtonView _attackButtonView;

    void Awake ()
    {
        _attackButtonView = GetComponentInChildren<AttackButtonView> ();
    }
    #endregion

    void Start ()
    {
        _attackButtonView.OnClick()
            .Subscribe(_=>
            {
                Gravitons.UI.Modal.ModalManager.Show(
                    "title",
                    "body",
                    new[]{ 
                        new Gravitons.UI.Modal.ModalButton{ Text = "OK", CloseModalOnClick = false, Callback = ShowModal},
                        new Gravitons.UI.Modal.ModalButton{ Text = "Cancel"}
                        }
                );
            });
    }

    void ShowModal()
    {
        Debug.Log("modal ok clicked");
    }
}