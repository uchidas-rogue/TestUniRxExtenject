using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FieldService : IDisposable
{
    public FieldService (int fieldWidth, int fieldHeight, int digX, int digY)
    {
        _width = fieldWidth;
        _height = fieldHeight;
        _x = digX;
        _y = digY;
        Field = new int[fieldWidth, fieldHeight, 2];
    }

    public void Dispose () { }

    #region Params
    int[, , ] Field { get; set; } = null;
    int _width;
    int _height;
    Direction _direction = Direction.right;
    int _x;
    int _y;
    int _roomWidth;
    int _roomHeiht;
    int _roomEntryX;
    int _roomEntryY;
    int _roomNumber;
    List<int[]> _floorPosList = new List<int[]> ();
    List<int[]> _stairsSuggestList = new List<int[]> ();
    Random _random = new Random ();

    #endregion // Params

    #region Method

    /// <summary>
    /// 指定の位置がFieldの内側かどうか
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool CheckInside0Position (int x, int y)
    {
        return (x > -1 && x < this._width) &&
            (y > -1 && y < this._height) &&
            (this.Field[x, y, 0] == 0);
    }

    /// <summary>
    /// 上下左右いづれかの方向に掘り進めるか
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool CheckCanDig (int x, int y)
    {
        return (CheckInside0Position (x, y + 2 * (int) Direction.up) ||
            CheckInside0Position (x, y + 2 * (int) Direction.down) ||
            CheckInside0Position (x + 2 * (int) Direction.left / Math.Abs ((int) Direction.left), y) ||
            CheckInside0Position (x + 2 * (int) Direction.right / Math.Abs ((int) Direction.right), y)
        );
    }

    /// <summary>
    /// 掘り進める方向に掘り進めるか
    /// </summary>
    /// <returns></returns>
    public bool CheckCanDigDirection ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            return CheckInside0Position (_x, _y + 2 * ((int) _direction / Math.Abs ((int) _direction)));
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            return CheckInside0Position (_x + 2 * ((int) _direction / Math.Abs ((int) _direction)), _y);
        }
    }

    /// <summary>
    /// 上下左右いづれかに部屋が作成できるか
    /// </summary>
    /// <returns></returns>
    public bool CheckCanMakeRoom ()
    {
        return (CheckInside0Position (_x - _roomEntryX, _y) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y) &&
                CheckInside0Position (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) Direction.up) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y + (_roomHeiht - 1) * (int) Direction.up)
            ) ||
            (CheckInside0Position (_x - _roomEntryX, _y) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y) &&
                CheckInside0Position (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) Direction.down) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y + (_roomHeiht - 1) * (int) Direction.down)
            ) ||
            (CheckInside0Position (_x, _y - _roomEntryY) &&
                CheckInside0Position (_x, _y - _roomEntryY + _roomHeiht - 1) &&
                CheckInside0Position (_x + (_roomWidth - 1) * (int) Direction.left / Math.Abs ((int) Direction.left), _y - _roomEntryY) &&
                CheckInside0Position (_x + (_roomWidth - 1) * (int) Direction.left / Math.Abs ((int) Direction.left), _y - _roomEntryY + _roomHeiht - 1)
            ) ||
            (CheckInside0Position (_x, _y - _roomEntryY) &&
                CheckInside0Position (_x, _y - _roomEntryY + _roomHeiht - 1) &&
                CheckInside0Position (_x + (_roomWidth - 1) * (int) Direction.right / Math.Abs ((int) Direction.right), _y - _roomEntryY) &&
                CheckInside0Position (_x + (_roomWidth - 1) * (int) Direction.right / Math.Abs ((int) Direction.right), _y - _roomEntryY + _roomHeiht - 1)
            );
    }

    /// <summary>
    /// 掘り進める方向に部屋が作成できるか
    /// </summary>
    /// <returns></returns>
    public bool CheckCanMakeRoomDirection ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            return (CheckInside0Position (_x - _roomEntryX, _y) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y) &&
                CheckInside0Position (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) _direction) &&
                CheckInside0Position (_x - _roomEntryX + _roomWidth - 1, _y + (_roomHeiht - 1) * (int) _direction)
            );
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            return (CheckInside0Position (_x, _y - _roomEntryY) &&
                CheckInside0Position (_x, _y - _roomEntryY + _roomHeiht - 1) &&
                CheckInside0Position (_x + (_roomWidth - 1) * ((int) _direction / Math.Abs ((int) _direction)), _y - _roomEntryY) &&
                CheckInside0Position (_x + (_roomWidth - 1) * ((int) _direction / Math.Abs ((int) _direction)), _y - _roomEntryY + _roomHeiht - 1)
            );
        }
    }

    /// <summary>
    /// 壁以外になってる場所を確認する
    /// </summary>
    void CheckFloorPosition ()
    {
        _floorPosList.Clear ();
        for (int i = 0; i < (this._width - 1) / 2; i++)
        {
            for (int j = 0; j < (this._height - 1) / 2; j++)
            {
                if (Field[2 * i + 1, 2 * j + 1, 0] != 0)
                {
                    _floorPosList.Add (new int[2] { 2 * i + 1, 2 * j + 1 });
                }
            }
        }
    }

    void CheckStairsSuggestPosition ()
    {
        CheckFloorPosition ();
        _stairsSuggestList.Clear ();
        foreach (var item in _floorPosList)
        {
            if ((Field[item[0] + 1, item[1], 0] + Field[item[0] - 1, item[1], 0] +
                    Field[item[0], item[1] + 1, 0] + Field[item[0], item[1] - 1, 0]) == 1)
            {
                _stairsSuggestList.Add (item);
            }
        }
    }

    public bool CheckAnyDigPosition ()
    {
        for (int i = 0; i < (this._width - 1) / 2; i++)
        {
            for (int j = 0; j < (this._height - 1) / 2; j++)
            {
                if (CheckCanDig (2 * i + 1, 2 * j + 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 掘り進める方向をランダムに変更する
    /// </summary>
    public void ChangeDir ()
    {
        switch (_random.Next (4))
        {
            case 0:
                this._direction = Direction.up;
                break;
            case 1:
                this._direction = Direction.down;
                break;
            case 2:
                this._direction = Direction.left;
                break;
            case 3:
                this._direction = Direction.right;
                break;
        }
    }

    /// <summary>
    /// 掘り進める始点位置をランダムに変更する
    /// </summary>
    public void ChangePosition ()
    {
        CheckFloorPosition ();
        int randomListNum = _random.Next (_floorPosList.Count);
        if (_floorPosList.Count != 0)
        {
            _x = _floorPosList[randomListNum][0];
            _y = _floorPosList[randomListNum][1];
        }
    }

    /// <summary>
    /// 部屋のサイズをランダムに変更する
    /// </summary>
    public void ChangeroomSize ()
    {
        _roomWidth = _random.Next (4, 8);
        if (_roomWidth % 2 == 0)
        { //odd num
            _roomWidth++;
        }
        _roomHeiht = _random.Next (4, 8);
        if (_roomHeiht % 2 == 0)
        { //odd num
            _roomHeiht++;
        }
    }

    /// <summary>
    /// 部屋の出入り口の場所をランダムに変更する
    /// </summary>
    public void ChangeroomEntry ()
    {
        _roomEntryX = _random.Next (_roomWidth + 1);
        if (_roomEntryX % 2 == 1)
        { //even num
            _roomEntryX--;
        }
        _roomEntryY = _random.Next (_roomHeiht + 1);
        if (_roomEntryY % 2 == 1)
        { //even num
            _roomEntryY--;
        }
    }

    public void MakeRoom ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            for (int i = 0; i < _roomWidth; i++)
            {
                for (int j = 0; j < _roomHeiht; j++)
                {
                    //(Field[x-entry,y]),(Field[x-entry,y+(size-1)])
                    //(Field[x-entry+size-1,y]),(Field[x-entry+size-1,y+(size-1)])
                    Field[_x - _roomEntryX + i, _y + j * (int) _direction, 0] = (int) FieldClass.floor;
                }
            }
            _roomNumber++;
        }
        else //if (direction == Direction.left || direction == Direction.right)
        {
            for (int i = 0; i < _roomWidth; i++)
            {
                for (int j = 0; j < _roomHeiht; j++)
                {
                    //(Field[x,y-entry]),(Field[x+(size-1),y-entry])
                    //(Field[x,y-entry+size-1]),(Field[x+(size-1),y-entry+size-1])
                    Field[_x + i * ((int) _direction / Math.Abs ((int) _direction)), _y - _roomEntryY + j, 0] = (int) FieldClass.floor;
                }
            }
            _roomNumber++;
        }
    }

    public void MakePath ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            _y += (int) _direction;
            Field[_x, _y, 0] = ((int) FieldClass.path);
            _y += (int) _direction;
            Field[_x, _y, 0] = ((int) FieldClass.path);
        }
        else // if (direction == Direction.left || direction == Direction.right)
        {
            _x += ((int) _direction / Math.Abs ((int) _direction));
            Field[_x, _y, 0] = ((int) FieldClass.path);
            _x += ((int) _direction / Math.Abs ((int) _direction));
            Field[_x, _y, 0] = ((int) FieldClass.path);
        }
    }

    public void MakeStairs ()
    {
        CheckStairsSuggestPosition ();
        if (_stairsSuggestList.Count != 0)
        {
            int randomListNum = _random.Next (_stairsSuggestList.Count);
            Field[_stairsSuggestList[randomListNum][0], _stairsSuggestList[randomListNum][1], 0] = ((int) FieldClass.exit);
        }
    }

    public int[, , ] MakeField (int floorNum)
    {
        // 最初の一部屋を作る
        if (CheckCanMakeRoom ())
        {
            while (!CheckCanMakeRoomDirection ())
            {
                ChangeDir ();
            }
            MakeRoom ();
        }
        int cnt = 0;
        while (cnt < (floorNum * 2 + 30) * floorNum)
        {
            ChangeDir ();
            ChangeroomSize ();
            ChangeroomEntry ();

            if (CheckCanMakeRoomDirection ())
            {
                MakeRoom ();
            }
            else if (CheckCanDigDirection ())
            {
                MakePath ();
            }
            else
            {
                ChangePosition ();
            }
            cnt++;
        }
        MakeStairs ();

        return Field;
    }

    public async UniTask<int[, , ]> MakeFieldAsync (int floorNum)
    {
        return await UniTask.Run (() => MakeField (floorNum));
    }

    #endregion // Method
}