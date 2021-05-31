using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class MiniMapPresenter : MonoBehaviour
{
    [SerializeField]
    MiniMapView _minimapview;

    IMiniMapModel _minimapmodel;
    IDangeonFieldModel _dangeonfieldmodel;
    IPlayerModel _playermodel;

    private StringBuilder mapStringBuilder = new StringBuilder ();

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IMiniMapModel injectmmm, IDangeonFieldModel injectdfm, IPlayerModel injectpm)
    {
        _minimapmodel = injectmmm;
        _dangeonfieldmodel = injectdfm;
        _playermodel = injectpm;
    }

    void Awake ()
    {
        _minimapview.OnClick ()
            .Subscribe (_ =>
            {
                _minimapmodel.isPickupRP.Value = !_minimapmodel.isPickupRP.Value;
            });

        _minimapmodel.isPickupRP
            .Subscribe (isPickup =>
            {
                if (isPickup)
                {
                    _minimapview.ChangeMapSize (new Vector3 (0f, 0f, 0f), new Vector2 (2220f, 1040f), 70f);
                }
                else
                {
                    _minimapview.ChangeMapSize (new Vector3 (720f, 320f, 0f), new Vector2 (800f, 420f), 20f);
                }
            });

        _playermodel.PlayerPositionVec3RP
            .Where (_ => _dangeonfieldmodel.Field != null)
            .Subscribe (ppos =>
            {
                _minimapview.SetMiniMapText (MakeMiniMapString ((int) ppos.x, (int) ppos.y));
            });
    }

    public void CheckFloor (int x, int y)
    {
        if (_dangeonfieldmodel.Field[x, y, 1] == 0)
        {// チェックしてないタイルなら
            _dangeonfieldmodel.Field[x, y, 1] = 1;
            if(_dangeonfieldmodel.Field[x, y, 0] == 2)
            {// まだフロア内なら
                TurnToWalked (x, y);
            }
        }
    }

    public void TurnToWalked (int x, int y)
    {
        if (_dangeonfieldmodel.Field[x, y, 0] == 2)
        {// フロア内なら周り全て見に行く
            CheckFloor (x - 1, y - 1);
            CheckFloor (x - 1, y);
            CheckFloor (x - 1, y + 1);
            CheckFloor (x, y - 1);
            CheckFloor (x, y + 1);
            CheckFloor (x + 1, y - 1);
            CheckFloor (x + 1, y);
            CheckFloor (x + 1, y + 1);
        }
        else
        {// フロア外なら問答無用で周り全てチェック済にする
            _dangeonfieldmodel.Field[x - 1, y - 1, 1] = 1;
            _dangeonfieldmodel.Field[x - 1, y, 1] = 1;
            _dangeonfieldmodel.Field[x - 1, y + 1, 1] = 1;
            _dangeonfieldmodel.Field[x, y - 1, 1] = 1;
            _dangeonfieldmodel.Field[x, y + 1, 1] = 1;
            _dangeonfieldmodel.Field[x + 1, y - 1, 1] = 1;
            _dangeonfieldmodel.Field[x + 1, y, 1] = 1;
            _dangeonfieldmodel.Field[x + 1, y + 1, 1] = 1;
        }
    }
    public string MakeMiniMapString (int playerposx, int playerposy)
    {
        mapStringBuilder.Clear ();

        TurnToWalked (playerposx, playerposy);

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