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
            _dangeonFieldModel.Item = makeFieldSevice.SetItems();
        }
        // 画面に設置する
        SetField ();
        SetItems ();
        // テスト用 ミニマップを全部表示
        //StartCheckWalkedTilesTest (49, 49);

        await UniTask.Delay (1000); // todo

        // floornum disappear
        //_changeFloorCanvasView.SetActiveAll (false);
        _dangeonFieldModel.IsFieldSetting = false;
    }

    void SetFieldSize ()
    {
        int width = _dangeonFieldView.FieldWidth;
        int heith = _dangeonFieldView.FieldHeith;
        // NG even number
        _dangeonFieldView.FieldWidth = width % 2 == 0 ? width + 1 : width;
        _dangeonFieldView.FieldHeith = heith % 2 == 0 ? heith + 1 : heith;

        width = _dangeonFieldView.FieldWidth;
        heith = _dangeonFieldView.FieldHeith;
        // NG size under 101
        _dangeonFieldView.FieldWidth = width < 101 ? 101 : width;
        _dangeonFieldView.FieldHeith = heith < 101 ? 101 : heith;
    }

    void SetField ()
    {
        int x = 0, y = 0;
        for (x = 0; x < _dangeonFieldModel.Field.GetLength (0); x++)
        {
            for (y = 0; y < _dangeonFieldModel.Field.GetLength (1); y++)
            {
                switch (_dangeonFieldModel.Field[x, y])
                {
                    case FieldClass.wall:
                        _dangeonFieldView.SetTile (_dangeonFieldView.WallTiles[0], Vector3.one, x, y, 0.8f);
                        break;
                    case FieldClass.path:
                    case FieldClass.floor:
                        _dangeonFieldView.SetTile (_dangeonFieldView.FloorTiles[0], Vector3.one, x, y);
                        break;
                    case FieldClass.exit:
                        _dangeonFieldView.SetTile (_dangeonFieldView.StairsTile, Vector3.one, x, y);
                        break;
                    default:
                        _dangeonFieldView.SetTile (_dangeonFieldView.FloorTiles[0], Vector3.one, x, y);
                        break;
                }
            }
        }
    }

    void SetItems ()
    {
        int x = 0, y = 0;
        for (x = 0; x < _dangeonFieldModel.Item.GetLength (0); x++)
        {
            for (y = 0; y < _dangeonFieldModel.Item.GetLength (1); y++)
            {
                switch (_dangeonFieldModel.Item[x, y])
                {
                    case ItemClass.potion:
                        _dangeonFieldView.SetTile (_dangeonFieldView.Items[0], Vector3.one * 2, x, y, 0.2f);
                        break;
                    default:
                        //_dangeonFieldView.SetTile (_dangeonFieldView.Items[0], x, y);
                        break;
                }
            }
        }
    }

}