using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class MiniMapPresenter : MonoBehaviour
{
    #region injection
    IMiniMapModel _miniMapModel;
    IMiniMapStringService _miniMapStringService;
    IDangeonFieldModel _dangeonFieldModel;
    IPlayerModel _playerModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (
        IMiniMapModel injectmmm, IMiniMapStringService injectmmss,
        IDangeonFieldModel injectdfm, IPlayerModel injectpm)
    {
        _miniMapModel = injectmmm;
        _miniMapStringService = injectmmss;
        _dangeonFieldModel = injectdfm;
        _playerModel = injectpm;
    }

    #endregion

    [SerializeField]
    MiniMapView _minimapview;

    void Awake ()
    {
        _minimapview.OnClick ()
            .ThrottleFirst (System.TimeSpan.FromSeconds (0.5f)) // 実行間隔の指定
            .DoOnSubscribe (() =>
            {
                MiniMapAction ();
            })
            .Subscribe (_ =>
            {
                _miniMapModel.IsPickup = !_miniMapModel.IsPickup;
                MiniMapAction (_miniMapModel.IsPickup);
            });
    }

    /// <summary>
    /// MiniMapがクリックされた時の処理
    /// </summary>
    /// <param name="isPickup"></param>
    private void MiniMapAction (bool isPickup = false)
    {

        _minimapview.SetMiniMapText (
            _miniMapStringService.MakeMiniMapString (
                (int) _playerModel.PlayerPositionVec3RP.Value.x,
                (int) _playerModel.PlayerPositionVec3RP.Value.y,
                _dangeonFieldModel.Field,
                isPickup
            ));

        if (isPickup)
        {
            _minimapview.ChangeMapSize (
                _miniMapModel.PickedMapPositionVec3, _miniMapModel.PiciedMapSizeVec2
            );
        }
        else
        {
            _minimapview.ChangeMapSize (
                _miniMapModel.MiniMapPositionVec3, _miniMapModel.MiniMapSizeVec2, false
            );
        }
    }

}