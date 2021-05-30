using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

public class DangeonFieldPresenter : MonoBehaviour
{
    //floor prefab 
    [SerializeField]
    public GameObject[] floorTiles;
    //wall prefab
    [SerializeField]
    public GameObject[] wallTiles;
    [SerializeField]
    public GameObject stairsTile;

    [SerializeField]
    public DangeonFieldVeiw _dangeonFieldView;
    private IDangeonFieldModel _dangeonFieldModel;

    [SerializeField]
    public int[] FieldSize;

    // zenjectによるDI、コンストラクタっぽく書くとエラーがでるらしい
    [Inject]
    public void Constructor (IDangeonFieldModel injectdfm)
    {
        _dangeonFieldModel = injectdfm;
    }

    void Awake ()
    {
        _dangeonFieldModel.FloorNumRP.Subscribe (
            num =>
            {
                Debug.Log ("floornum:" + num);
                SetFieldSize(num);
                _dangeonFieldView.RemoveAllTiles ();
                _dangeonFieldModel.MakeField (FieldSize[0], FieldSize[1]);
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
        FieldSize[0] += floornum;
        FieldSize[1] += floornum;

        // NG size under 7
        FieldSize[0] = FieldSize[0] < 11 ? 11 : FieldSize[0];
        FieldSize[1] = FieldSize[1] < 11 ? 11 : FieldSize[1];
        // NG even number
        FieldSize[0] = FieldSize[0] % 2 == 0 ? FieldSize[0] + 1 : FieldSize[0];
        FieldSize[1] = FieldSize[1] % 2 == 0 ? FieldSize[1] + 1 : FieldSize[1];
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
                        _dangeonFieldView.SetTile (wallTiles[(int) Wall.wallreaf], x, y);
                        break;
                    case 1:
                        _dangeonFieldView.SetTile (floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case 2:
                        _dangeonFieldView.SetTile (floorTiles[(int) Floor.rocktile], x, y);
                        break;
                    case 3:
                        _dangeonFieldView.SetTile (stairsTile, x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}