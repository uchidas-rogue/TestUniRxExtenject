using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class MiniMapPresenter : MonoBehaviour
{
    #region injection
    IMiniMapModel _minimapmodel;
    IDangeonFieldModel _dangeonfieldmodel;
    IPlayerModel _playermodel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IMiniMapModel injectmmm, IDangeonFieldModel injectdfm, IPlayerModel injectpm)
    {
        _minimapmodel = injectmmm;
        _dangeonfieldmodel = injectdfm;
        _playermodel = injectpm;
    }

    #endregion

    [SerializeField]
    MiniMapView _minimapview;
    private StringBuilder mapStringBuilder = new StringBuilder ();

    void Awake ()
    {
        _minimapview.OnClick ()
            .Subscribe (_ =>
            {
                _minimapmodel.IsPickupRP.Value = !_minimapmodel.IsPickupRP.Value;
            });

        _minimapmodel.IsPickupRP
            .Subscribe (isPickup =>
            {
                if (isPickup)
                {
                    _minimapview.ChangeMapSize (
                        _minimapmodel.PickedMapPositionVec3, _minimapmodel.PiciedMapSizeVec2, 70f
                    );
                }
                else
                {
                    _minimapview.ChangeMapSize (
                        _minimapmodel.MiniMapPositionVec3, _minimapmodel.MiniMapSizeVec2, 20f
                    );
                }
            });

        _playermodel.PlayerPositionVec3RP
            .Where (_ => _dangeonfieldmodel.Field != null)
            .Subscribe (ppos =>
            {
                _minimapview.SetMiniMapText (MakeMiniMapString ((int) ppos.x, (int) ppos.y));
            });
    }

    /// <summary>
    /// 歩いた場所かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CheckWalkedTile (int x, int y)
    {
        if (_dangeonfieldmodel.Field[x, y, 1] == 0)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonfieldmodel.Field[x, y, 1] = 1;
            if (_dangeonfieldmodel.Field[x, y, 0] == 2)
            { // まだフロア内なら
                // さらに周りを調べに行く
                StartCheckWalkedTiles (x, y);
            }
        }
    }

    public void StartCheckWalkedTiles (int x, int y)
    {
        // 八方向全てチェックしに行く
        CheckWalkedTile (x - 1, y - 1);
        CheckWalkedTile (x - 1, y);
        CheckWalkedTile (x - 1, y + 1);
        CheckWalkedTile (x, y - 1);
        CheckWalkedTile (x, y + 1);
        CheckWalkedTile (x + 1, y - 1);
        CheckWalkedTile (x + 1, y);
        CheckWalkedTile (x + 1, y + 1);
    }
    public string MakeMiniMapString (int playerposx, int playerposy)
    {
        mapStringBuilder.Clear ();
        StartCheckWalkedTiles (playerposx, playerposy);

        for (int x = _dangeonfieldmodel.Field.GetLength (0) - 1; x >= 0; x--)
        {
            for (int y = 0; y < _dangeonfieldmodel.Field.GetLength (1); y++)
            {
                if (y == playerposx && x == playerposy)
                { //player position
                    mapStringBuilder.Append ("<color=yellow>●</color>");
                }
                else if (_dangeonfieldmodel.Field[y, x, 0] == 3)
                { //exit position
                    if (_dangeonfieldmodel.Field[y, x, 1] == 1)
                    {
                        mapStringBuilder.Append ("<color=green>■</color>");
                    }
                    else
                    {
                        mapStringBuilder.Append ("   ");
                    }
                }
                else if (_dangeonfieldmodel.Field[y, x, 0] == 1 || _dangeonfieldmodel.Field[y, x, 0] == 2)
                { //floor position
                    if (_dangeonfieldmodel.Field[y, x, 1] == 1)
                    {
                        mapStringBuilder.Append ("<color=blue>■</color>");
                    }
                    else
                    {
                        mapStringBuilder.Append ("   ");
                    }
                }
                else
                { //wall position
                    if (_dangeonfieldmodel.Field[y, x, 1] == 1)
                    {
                        mapStringBuilder.Append ("■");
                    }
                    else
                    {
                        mapStringBuilder.Append ("   ");
                    }
                }
            }
            mapStringBuilder.AppendLine ("");
        }
        return mapStringBuilder.ToString ();
    }

}