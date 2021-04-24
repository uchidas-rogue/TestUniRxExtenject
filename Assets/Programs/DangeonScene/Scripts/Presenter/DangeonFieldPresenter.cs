using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using System.Threading;

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
    public void Constructor(IDangeonFieldModel inject)
    {
        _dangeonFieldModel = inject;
    }

    void Awake()
    {
        _dangeonFieldModel.FloorNumRP.Subscribe(
            num => {
                Debug.Log(num);
                _dangeonFieldView.RemoveAllTiles();
                _dangeonFieldModel.MakeField(FieldSize[0],FieldSize[1]);
                SetField();
                }
        );

        Observable.Timer(System.TimeSpan.FromSeconds(5), System.TimeSpan.FromSeconds(4))
                    .Subscribe(_=>_dangeonFieldModel.FloorNumRP.Value++)
                    ;
        
    }

    public void SetField()
    {
        for (int x = 0; x < _dangeonFieldModel.Field.GetLength(0); x++)
        {
            for (int y = 0; y < _dangeonFieldModel.Field.GetLength(1); y++)
            {
                switch (_dangeonFieldModel.Field[x, y, 0])
                {
                    case 0:
                        _dangeonFieldView.SetTile (wallTiles[0], x, y);
                        break;
                    case 1:
                        _dangeonFieldView.SetTile (floorTiles[0], x, y);
                        break;
                    case 2:
                        _dangeonFieldView.SetTile (floorTiles[0], x, y);
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
