using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    public DangeonFieldVeiw DangeonFieldView;

    [SerializeField]
    public int FieldWidth;
    [SerializeField]
    public int FieldHeith;
    [SerializeField]
    public ChangeFloorCanvasView ChangeFloorCanvasView;

    void Awake ()
    {
        _dangeonFieldModel.FloorNumRP
            .DoOnSubscribe (SetFieldSize)
            .Subscribe (num => InitField (num));

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

    private async void InitField (int num)
    {
        _dangeonFieldModel.IsFieldSetting = true;
        // 画面に設置済みのタイルを全て消す
        DangeonFieldView.RemoveAllTiles ();

        // floornum appear
        ChangeFloorCanvasView.SetActiveAll(true);
        ChangeFloorCanvasView.SetFloorNumText ($"FloorNum:{num}");

        using (var makeFieldSevice = new FieldService (FieldWidth, FieldHeith, 49, 49))
        {
            _dangeonFieldModel.Field = await makeFieldSevice.MakeFieldAsync (num);
        }
        // 画面に設置する
        SetField ();
        // テスト用 ミニマップを全部表示
        //StartCheckWalkedTilesTest (49, 49);

        await UniTask.Delay (1000);

        // floornum disappear
        ChangeFloorCanvasView.SetActiveAll(false);
        _dangeonFieldModel.IsFieldSetting = false;
    }

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
                        DangeonFieldView.SetTile (DangeonFieldView.WallTiles[(int) Wall.wallreaf], x, y);
                        break;
                    case (int) FieldClass.path:
                        DangeonFieldView.SetTile (DangeonFieldView.FloorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case (int) FieldClass.floor:
                        DangeonFieldView.SetTile (DangeonFieldView.FloorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case (int) FieldClass.exit:
                        DangeonFieldView.SetTile (DangeonFieldView.StairsTile, x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}