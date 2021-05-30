using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

public class DangeonFieldPresenter : MonoBehaviour
{
    [SerializeField]
    public DangeonFieldVeiw _dangeonFieldView;
    private IDangeonFieldModel _dangeonFieldModel;

    private IPlayerModel _playermodel;

    [SerializeField]
    public int FieldWidth;
     [SerializeField]
    public int FieldHeith;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IDangeonFieldModel injectdfm,IPlayerModel injectpm)
    {
        _dangeonFieldModel = injectdfm;
        _playermodel = injectpm;
    }

    void Awake ()
    {
        _dangeonFieldModel.FloorNumRP.Subscribe (
            num =>
            {
                Debug.Log ("floornum:" + num);
                SetFieldSize(num);
                _dangeonFieldView.RemoveAllTiles ();
                _dangeonFieldModel.MakeField (FieldWidth, FieldHeith);
                SetField ();
            }
        );

        // Observable.Timer (System.TimeSpan.FromSeconds (2), System.TimeSpan.FromSeconds (4))
        //     .Subscribe (_ =>
        //     {
        //         Debug.Log (_dangeonFieldModel.FloorNumRP.Value);
        //         _dangeonFieldModel.FloorNumRP.Value++;
        //     });

    }

    public void SetFieldSize (int floornum)
    {
        FieldWidth += floornum;
        FieldHeith += floornum;

        // NG size under 7
        FieldWidth = FieldWidth < 11 ? 11 : FieldWidth;
        FieldHeith = FieldHeith < 11 ? 11 : FieldHeith;
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
                    case 0:
                        _dangeonFieldView.SetTile (_dangeonFieldView.wallTiles[(int) Wall.wallreaf], x, y);
                        break;
                    case 1:
                        _dangeonFieldView.SetTile (_dangeonFieldView.floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case 2:
                        _dangeonFieldView.SetTile (_dangeonFieldView.floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case 3:
                        _dangeonFieldView.SetTile (_dangeonFieldView.stairsTile, x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}