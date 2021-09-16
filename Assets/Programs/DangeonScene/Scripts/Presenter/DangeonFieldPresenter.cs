using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class DangeonFieldPresenter : MonoBehaviour
{
    #region injection
    private IDangeonFieldModel _dangeonFieldModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IDangeonFieldModel injectdfm)
    {
        _dangeonFieldModel = injectdfm;
    }
    #endregion

    [SerializeField]
    public DangeonFieldVeiw _dangeonFieldView;

    [SerializeField]
    public int FieldWidth;
    [SerializeField]
    public int FieldHeith;

    void Awake ()
    {
         _dangeonFieldModel.FloorNumRP
            .Subscribe (
                num =>
                {
                    _dangeonFieldModel.IsFieldSetting = true;

                    SetFieldSize ();
                    _dangeonFieldView.RemoveAllTiles ();
                    _dangeonFieldModel.MakeField (FieldWidth, FieldHeith, num);
                    SetField ();
                    //StartCheckWalkedTilesTest(49,49);

                    _dangeonFieldModel.IsFieldSetting = false;
                }
            );

        // Observable.Timer (System.TimeSpan.FromSeconds (2), System.TimeSpan.FromSeconds (4))
        //     .Subscribe (_ =>
        //     {
        //         _dangeonFieldModel.FloorNumRP.Value++;
        //     });

    }

    #region WalkedMapTest

    /// <summary>
    /// 歩いた場所かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CheckWalkedTileTest (int x, int y)
    {
        if (_dangeonFieldModel.Field[x, y, 1] == 0)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonFieldModel.Field[x, y, 1] = 1;
            if (_dangeonFieldModel.Field[x, y, 0] != 0)
            { // 壁以外なら
                // さらに周りを調べに行く
                StartCheckWalkedTilesTest (x, y);
            }
        }
    }

    public void StartCheckWalkedTilesTest (int x, int y)
    {
        // 八方向全てチェックしに行く
        CheckWalkedTileTest (x - 1, y - 1);
        CheckWalkedTileTest (x - 1, y);
        CheckWalkedTileTest (x - 1, y + 1);
        CheckWalkedTileTest (x, y - 1);
        CheckWalkedTileTest (x, y + 1);
        CheckWalkedTileTest (x + 1, y - 1);
        CheckWalkedTileTest (x + 1, y);
        CheckWalkedTileTest (x + 1, y + 1);
    }
    #endregion

    public void SetFieldSize ()
    {
        // NG size under 101
        FieldWidth = FieldWidth < 101 ? 101 : FieldWidth;
        FieldHeith = FieldHeith < 101 ? 101 : FieldHeith;
        // NG even number
        FieldWidth = FieldWidth % 2 == 0 ? FieldWidth + 1 : FieldWidth;
        FieldHeith = FieldHeith % 2 == 0 ? FieldHeith + 1 : FieldHeith;
    }

    public void SetField ()
    {
        for (int x = 0; x < _dangeonFieldModel.Field.GetLength (0); x++)
        {
            for (int y = 0; y < _dangeonFieldModel.Field.GetLength (1); y++)
            {
                switch (_dangeonFieldModel.Field[x, y, 0])
                {
                    case (int) FieldClass.wall:
                        _dangeonFieldView.SetTile (_dangeonFieldView.wallTiles[(int) Wall.wallreaf], x, y);
                        break;
                    case (int) FieldClass.path:
                        _dangeonFieldView.SetTile (_dangeonFieldView.floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case (int) FieldClass.floor:
                        _dangeonFieldView.SetTile (_dangeonFieldView.floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case (int) FieldClass.exit:
                        _dangeonFieldView.SetTile (_dangeonFieldView.stairsTile, x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}