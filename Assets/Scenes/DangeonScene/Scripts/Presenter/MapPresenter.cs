using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class MapPresenter : MonoBehaviour
{
    #region injection
    IMapStringService _mapStringService;
    IDangeonFieldModel _dangeonFieldModel;
    IPlayerModel _playerModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IMapStringService mss, IDangeonFieldModel dfm, IPlayerModel pm)
    {
        _mapStringService = mss;
        _dangeonFieldModel = dfm;
        _playerModel = pm;
    }

    #endregion // injection

    #region views
    MapView _mapView;

    void Awake ()
    {
        _mapView = GetComponent<MapView> ();
    }

    #endregion // views

    void Start ()
    {
        _mapView.OnClick ()
            .DoOnSubscribe (() => MapAction ())
            .ThrottleFirst (System.TimeSpan.FromSeconds (0.5f)) // 実行間隔の指定
            .Subscribe (_ =>
            {
                MapAction (_mapView.MapSizeVec2 == _mapView.MapSize);
            });
    }

    /// <summary>
    /// Mapがクリックされた時の処理
    /// </summary>
    /// <param name="isPickup"></param>
    void MapAction (bool isPickup = false)
    {

        _mapView.SetMapText (
            _mapStringService.MakeMapString (
                (int) _playerModel.PlayerPositionVec3RP.Value.x,
                (int) _playerModel.PlayerPositionVec3RP.Value.z,
                isPickup
            ));

        if (isPickup)
        {
            _mapView.ChangeMapSize (
                _mapView.PickedMapPositionVec3, _mapView.PickedMapSizeVec2
            );
        }
        else
        {
            _mapView.ChangeMapSize (
                _mapView.MapPositionVec3, _mapView.MapSizeVec2, false
            );
        }
    }

}