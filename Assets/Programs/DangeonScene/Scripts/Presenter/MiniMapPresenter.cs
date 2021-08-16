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
    IMiniMapModel _minimapModel;
    IDangeonFieldModel _dangeonFieldModel;
    IPlayerModel _playerModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IMiniMapModel injectmmm, IDangeonFieldModel injectdfm, IPlayerModel injectpm)
    {
        _minimapModel = injectmmm;
        _dangeonFieldModel = injectdfm;
        _playerModel = injectpm;
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
                _minimapModel.IsPickupRP.Value = !_minimapModel.IsPickupRP.Value;
            });

        _minimapModel.IsPickupRP
            .Subscribe (isPickup =>
            {
                if (isPickup)
                {
                    if (_dangeonFieldModel.Field != null)
                    {
                        _minimapview.SetMiniMapText (
                            this.MakeMiniMapPickedString (
                                (int) _playerModel.PlayerPositionVec3RP.Value.x, (int) _playerModel.PlayerPositionVec3RP.Value.y)
                        );
                    }
                    _minimapview.ChangeMapSize (
                        _minimapModel.PickedMapPositionVec3, _minimapModel.PiciedMapSizeVec2
                    );
                }
                else
                {
                    if (_dangeonFieldModel.Field != null)
                    {
                        _minimapview.SetMiniMapText (
                            this.MakeMiniMapString (
                                (int) _playerModel.PlayerPositionVec3RP.Value.x, (int) _playerModel.PlayerPositionVec3RP.Value.y)
                        );
                    }
                    _minimapview.ChangeMapSize (
                        _minimapModel.MiniMapPositionVec3, _minimapModel.MiniMapSizeVec2, false
                    );
                }
            });

        _playerModel.PlayerPositionVec3RP
            .Where (_ => _dangeonFieldModel.Field != null)
            .Subscribe (ppos =>
            {
                StartCheckWalkedTiles ((int) ppos.x, (int) ppos.y);
                _minimapview.SetMiniMapText (this.MakeMiniMapString ((int) ppos.x, (int) ppos.y));
            });
    }

    /// <summary>
    /// 歩いた場所かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CheckWalkedTile (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y, 1] == 0)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonFieldModel.Field[x, y, 1] = 1;
            if (_dangeonFieldModel.Field[x, y, 0] == 2)
            { // まだフロア内なら
                // さらに周りを調べに行く
                StartCheckWalkedTiles (x, y);
            }
        }
    }

    public void StartCheckWalkedTiles (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y, 0] == 2)
        { // player in floor
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
        else
        { // これないとフロアに入る前にフロアがマップにでる
            _dangeonFieldModel.Field[x - 1, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x - 1, y, 1] = 1;
            _dangeonFieldModel.Field[x - 1, y + 1, 1] = 1;
            _dangeonFieldModel.Field[x, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x, y + 1, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y - 1, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y, 1] = 1;
            _dangeonFieldModel.Field[x + 1, y + 1, 1] = 1;
        }

    }

    public string MakeMiniMapString (int playerposx, int playerposy)
    {
        mapStringBuilder.Clear ();

        for (int y = playerposy + 7; y >= playerposy - 7; y--)
        {
            for (int x = playerposx - 7; x <= playerposx + 7; x++)
            {
                ConvObjtoRichtext (playerposx, playerposy, x, y);
            }
            mapStringBuilder.AppendLine ("");
        }
        return mapStringBuilder.ToString ();
    }

    public string MakeMiniMapPickedString (int playerposx, int playerposy)
    {
        mapStringBuilder.Clear ();

        for (int y = _dangeonFieldModel.Field.GetLength (0) - 1; y >= 0; y--)
        {
            for (int x = 0; x < _dangeonFieldModel.Field.GetLength (1); x++)
            {
                ConvObjtoRichtext (playerposx, playerposy, x, y);
            }
            mapStringBuilder.AppendLine ("");
        }
        return mapStringBuilder.ToString ();
    }

    /// <summary>
    /// それぞれのオブジェクトをリッチテキストに置き換える
    /// </summary>
    /// <param name="playerposx"></param>
    /// <param name="playerposy"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ConvObjtoRichtext (int playerposx, int playerposy, int x, int y)
    {
        if (x == playerposx && y == playerposy)
        { //player position
            mapStringBuilder.Append ("<color=yellow>●</color>");
        }
        else if (_dangeonFieldModel.Field[x, y, 0] == 3)
        { //exit position
            if (_dangeonFieldModel.Field[x, y, 1] == 1)
            {
                mapStringBuilder.Append ("<color=green>■</color>");
            }
            else
            {
                mapStringBuilder.Append ("   ");
            }
        }
        else if (_dangeonFieldModel.Field[x, y, 0] == 1 || _dangeonFieldModel.Field[x, y, 0] == 2)
        { //floor position
            if (_dangeonFieldModel.Field[x, y, 1] == 1)
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
            if (_dangeonFieldModel.Field[x, y, 1] == 1)
            {
                mapStringBuilder.Append ("■");
            }
            else
            {
                mapStringBuilder.Append ("   ");
            }
        }
    }

}