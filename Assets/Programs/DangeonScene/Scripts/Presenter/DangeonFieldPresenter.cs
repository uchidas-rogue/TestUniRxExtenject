using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

public class DangeonFieldPresenter : MonoBehaviour
{
    #region injection
    private IDangeonFieldModel _dangeonFieldModel;
    private IPlayerModel _playermodel;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IDangeonFieldModel injectdfm, IPlayerModel injectpm)
    {
        _dangeonFieldModel = injectdfm;
        _playermodel = injectpm;
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
        _dangeonFieldModel.FloorNumRP.Subscribe (
            num =>
            {
                _dangeonFieldModel.IsFieldSetting.Value = true;
                Debug.Log ("floornum:" + num);
                SetFieldSize ();
                _dangeonFieldView.RemoveAllTiles ();
                _dangeonFieldModel.MakeField (FieldWidth, FieldHeith, num);
                SetField ();
                _dangeonFieldModel.IsFieldSetting.Value = false;
            }
        );

        // Observable.Timer (System.TimeSpan.FromSeconds (2), System.TimeSpan.FromSeconds (4))
        //     .Subscribe (_ =>
        //     {
        //         _dangeonFieldModel.FloorNumRP.Value++;
        //     });

    }

    public void SetFieldSize ()
    {
        // NG size under 7
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