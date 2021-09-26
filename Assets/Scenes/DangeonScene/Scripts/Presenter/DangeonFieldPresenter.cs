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
    IDangeonFieldModel _dangeonFieldModel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IDangeonFieldModel injectdfm)
    {
        _dangeonFieldModel = injectdfm;
    }
    #endregion // injection

    #region views

    DangeonFieldVeiw _dangeonFieldView;
    //ChangeFloorCanvasView _changeFloorCanvasView;

    void Awake ()
    {
        _dangeonFieldView = GetComponent<DangeonFieldVeiw> ();
        //_changeFloorCanvasView = FindObjectOfType<ChangeFloorCanvasView> ();
    }
    #endregion // views

    void Start ()
    {
        // Destoroy時にキャンセルされるtoken
        var cancelToken = this.GetCancellationTokenOnDestroy ();

        _dangeonFieldModel.FloorNumRP
            .DoOnSubscribe (SetFieldSize)
            .Subscribe (num => InitField (num, cancelToken));

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
        if (_dangeonFieldModel.Map[x, y] != MapClass.walked)
        { // チェックしてないタイルなら
            // チェック済にする
            _dangeonFieldModel.Map[x, y] = MapClass.walked;
            if (_dangeonFieldModel.Field[x, y] != FieldClass.wall)
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

    async void InitField (int num, CancellationToken cancellationToken)
    {
        _dangeonFieldModel.IsFieldSetting = true;
        // 画面に設置済みのタイルを全て消す
        _dangeonFieldView.RemoveAllTiles ();

        // floornum appear
        // _changeFloorCanvasView.SetActiveAll (true);
        // _changeFloorCanvasView.SetFloorNumText ($"FloorNum:{num}");

        using (var makeFieldSevice = new FieldService (_dangeonFieldView.FieldWidth, _dangeonFieldView.FieldHeith, 49, 49))
        {
            _dangeonFieldModel.Field = await makeFieldSevice.MakeFieldAsync (num, cancellationToken);
            _dangeonFieldModel.Map = new MapClass[_dangeonFieldView.FieldWidth, _dangeonFieldView.FieldHeith];
        }
        // 画面に設置する
        SetField ();
        // テスト用 ミニマップを全部表示
        //StartCheckWalkedTilesTest (49, 49);

        await UniTask.Delay (1000); // todo

        // floornum disappear
        //_changeFloorCanvasView.SetActiveAll (false);
        _dangeonFieldModel.IsFieldSetting = false;
    }

    void SetFieldSize ()
    {
        // NG size under 101
        _dangeonFieldView.FieldWidth = _dangeonFieldView.FieldWidth < 101 ? 101 : _dangeonFieldView.FieldWidth;
        _dangeonFieldView.FieldHeith = _dangeonFieldView.FieldHeith < 101 ? 101 : _dangeonFieldView.FieldHeith;
        // NG even number
        _dangeonFieldView.FieldWidth = _dangeonFieldView.FieldWidth % 2 == 0 ? _dangeonFieldView.FieldWidth + 1 : _dangeonFieldView.FieldWidth;
        _dangeonFieldView.FieldHeith = _dangeonFieldView.FieldHeith % 2 == 0 ? _dangeonFieldView.FieldHeith + 1 : _dangeonFieldView.FieldHeith;
    }

    void SetField ()
    {
        for (int x = 0; x < _dangeonFieldModel.Field.GetLength (0); x++)
        {
            for (int y = 0; y < _dangeonFieldModel.Field.GetLength (1); y++)
            {
                switch (_dangeonFieldModel.Field[x, y])
                {
                    case FieldClass.wall:
                        _dangeonFieldView.SetTile (_dangeonFieldView.WallTiles[(int) Wall.reaf], x, y, 0.8f);
                        break;
                    case FieldClass.path:
                        _dangeonFieldView.SetTile (_dangeonFieldView.FloorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case FieldClass.floor:
                        _dangeonFieldView.SetTile (_dangeonFieldView.FloorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case FieldClass.exit:
                        _dangeonFieldView.SetTile (_dangeonFieldView.StairsTile, x, y);
                        break;
                    default:
                        _dangeonFieldView.SetTile (_dangeonFieldView.FloorTiles[(int) Floor.rocktile], x, y);
                        break;
                }
            }
        }
    }

}