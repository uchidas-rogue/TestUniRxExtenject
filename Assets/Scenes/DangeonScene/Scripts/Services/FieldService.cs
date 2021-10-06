using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class FieldService : IDisposable
{
    public FieldService (int fieldWidth, int fieldHeight, int digX, int digY)
    {
        _width = fieldWidth;
        _height = fieldHeight;
        _initX = digX;
        _initY = digY;
        _x = digX;
        _y = digY;
        Field = new FieldClass[fieldWidth, fieldHeight];
    }

    public void Dispose () { }

    #region Params
    FieldClass[, ] Field { get; set; } = null;
    int _width;
    int _height;
    Direction _direction = Direction.right;
    int _initX;
    int _initY;
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
    bool CheckInsideWallPosition (int x, int y)
    {
        return (x > -1 && x < this._width) &&
            (y > -1 && y < this._height) &&
            (this.Field[x, y] == FieldClass.wall);
    }

    /// <summary>
    /// 上下左右いづれかの方向に掘り進めるか
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool CheckCanDig (int x, int y)
    {
        return (CheckInsideWallPosition (x, y + 2 * (int) Direction.up) ||
            CheckInsideWallPosition (x, y + 2 * (int) Direction.down) ||
            CheckInsideWallPosition (x + 2 * (int) Direction.left / Math.Abs ((int) Direction.left), y) ||
            CheckInsideWallPosition (x + 2 * (int) Direction.right / Math.Abs ((int) Direction.right), y)
        );
    }

    /// <summary>
    /// 掘り進める方向に掘り進めるか
    /// </summary>
    /// <returns></returns>
    bool CheckCanDigDirection ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            return CheckInsideWallPosition (_x, _y + 2 * ((int) _direction / Math.Abs ((int) _direction)));
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            return CheckInsideWallPosition (_x + 2 * ((int) _direction / Math.Abs ((int) _direction)), _y);
        }
    }

    /// <summary>
    /// 上下左右いづれかに部屋が作成できるか
    /// </summary>
    /// <returns></returns>
    bool CheckCanMakeRoom ()
    {
        return (CheckInsideWallPosition (_x - _roomEntryX, _y) &&
                CheckInsideWallPosition (_x - _roomEntryX + _roomWidth - 1, _y) &&
                CheckInsideWallPosition (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) Direction.up) &&
                CheckInsideWallPosition (_x - _roomEntryX + _roomWidth - 1, _y + (_roomHeiht - 1) * (int) Direction.up)
            ) ||
            (CheckInsideWallPosition (_x - _roomEntryX, _y) &&
                CheckInsideWallPosition (_x - _roomEntryX + _roomWidth - 1, _y) &&
                CheckInsideWallPosition (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) Direction.down) &&
                CheckInsideWallPosition (_x - _roomEntryX + _roomWidth - 1, _y + (_roomHeiht - 1) * (int) Direction.down)
            ) ||
            (CheckInsideWallPosition (_x, _y - _roomEntryY) &&
                CheckInsideWallPosition (_x, _y - _roomEntryY + _roomHeiht - 1) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * (int) Direction.left / Math.Abs ((int) Direction.left), _y - _roomEntryY) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * (int) Direction.left / Math.Abs ((int) Direction.left), _y - _roomEntryY + _roomHeiht - 1)
            ) ||
            (CheckInsideWallPosition (_x, _y - _roomEntryY) &&
                CheckInsideWallPosition (_x, _y - _roomEntryY + _roomHeiht - 1) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * (int) Direction.right / Math.Abs ((int) Direction.right), _y - _roomEntryY) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * (int) Direction.right / Math.Abs ((int) Direction.right), _y - _roomEntryY + _roomHeiht - 1)
            );
    }

    /// <summary>
    /// 掘り進める方向に部屋が作成できるか
    /// </summary>
    /// <returns></returns>
    bool CheckCanMakeRoomDirection ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            //(Field[x-entry,y]),(Field[x-entry,y+(size-1)])
            //(Field[x-entry+size-1,y]),(Field[x-entry+size-1,y+(size-1)])
            return (CheckInsideWallPosition (_x - _roomEntryX, _y) &&
                CheckInsideWallPosition (_x - _roomEntryX + (_roomWidth - 1), _y) &&
                CheckInsideWallPosition (_x - _roomEntryX, _y + (_roomHeiht - 1) * (int) _direction) &&
                CheckInsideWallPosition (_x - _roomEntryX + (_roomWidth - 1), _y + (_roomHeiht - 1) * (int) _direction)
            );
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            //(Field[x,y-entry]),(Field[x+(size-1),y-entry])
            //(Field[x,y-entry+size-1]),(Field[x+(size-1),y-entry+size-1])
            return (CheckInsideWallPosition (_x, _y - _roomEntryY) &&
                CheckInsideWallPosition (_x, _y - _roomEntryY + (_roomHeiht - 1)) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * ((int) _direction / Math.Abs ((int) _direction)), _y - _roomEntryY) &&
                CheckInsideWallPosition (_x + (_roomWidth - 1) * ((int) _direction / Math.Abs ((int) _direction)), _y - _roomEntryY + (_roomHeiht - 1))
            );
        }
    }

    /// <summary>
    /// 壁以外になってる場所を確認する
    /// </summary>
    void CheckFloorPosition ()
    {
        int i, j;
        _floorPosList.Clear ();
        for (i = 0; i < (this._width - 1) / 2; i++)
        {
            for (j = 0; j < (this._height - 1) / 2; j++)
            {
                if (Field[2 * i + 1, 2 * j + 1] != FieldClass.wall)
                {
                    _floorPosList.Add (new int[2] { 2 * i + 1, 2 * j + 1 });
                }
            }
        }
    }

    void CheckStairSuggestPosition ()
    {
        CheckFloorPosition ();
        _stairsSuggestList.Clear ();
        foreach (var item in _floorPosList)
        {
            if (((int) Field[item[0] + 1, item[1]] +
                    (int) Field[item[0] - 1, item[1]] +
                    (int) Field[item[0], item[1] + 1] +
                    (int) Field[item[0], item[1] - 1]
                ) == 1)
            {
                _stairsSuggestList.Add (item);
            }
        }
    }

    bool CheckAnyDigPosition ()
    {
        int i, j;
        for (i = 0; i < (this._width - 1) / 2; i++)
        {
            for (j = 0; j < (this._height - 1) / 2; j++)
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
    void ChangeDir ()
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
    void ChangePosition ()
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
    void ChangeroomSize ()
    {
        _roomWidth = _random.Next (4, 8);
        if (_roomWidth % 2 == 0) { _roomWidth++; }
        _roomHeiht = _random.Next (4, 8);
        if (_roomHeiht % 2 == 0) { _roomHeiht++; }
    }

    /// <summary>
    /// 部屋の出入り口の場所をランダムに変更する
    /// </summary>
    void ChangeroomEntry ()
    {
        _roomEntryX = _random.Next (_roomWidth + 1);
        if (_roomEntryX % 2 == 1) { _roomEntryX--; }
        _roomEntryY = _random.Next (_roomHeiht + 1);
        if (_roomEntryY % 2 == 1) { _roomEntryY--; }
    }

    void MakeRoomSub (int xroomdigstat, int yroomdigstat, int dirx, int diry)
    {
        int i, j;

        for (i = 0; i < _roomWidth; i++)
        {
            for (j = 0; j < _roomHeiht; j++)
            {
                Field[xroomdigstat + (i * dirx), yroomdigstat + (j * diry)] = FieldClass.floor;
            }
        }
    }

    void MakeRoom ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            //(Field[x-entry+0,y]),(Field[x-entry,y+(size-1)])
            //(Field[x-entry+(size-1),y]),(Field[x-entry+(size-1),y+(size-1)])
            MakeRoomSub (
                _x - _roomEntryX, _y,
                1, ((int) _direction / Math.Abs ((int) _direction))
            );
        }
        else // if (direction == Direction.left || direction == Direction.right)
        {
            //(Field[x,y-entry]),(Field[x+(size-1),y-entry])
            //(Field[x,y-entry+size-1]),(Field[x+(size-1),y-entry+size-1])
            MakeRoomSub (
                _x, _y - _roomEntryY,
                ((int) _direction / Math.Abs ((int) _direction)), 1
            );
        }
    }

    void MakePath ()
    {
        if (_direction == Direction.up || _direction == Direction.down)
        {
            _y += (int) _direction;
            Field[_x, _y] = (FieldClass.path);
            _y += (int) _direction;
            Field[_x, _y] = (FieldClass.path);
        }
        else // if (direction == Direction.left || direction == Direction.right)
        {
            _x += ((int) _direction / Math.Abs ((int) _direction));
            Field[_x, _y] = (FieldClass.path);
            _x += ((int) _direction / Math.Abs ((int) _direction));
            Field[_x, _y] = (FieldClass.path);
        }
    }

    bool MakeStairs ()
    {
        CheckStairSuggestPosition ();
        if (_stairsSuggestList.Count != 0)
        {
            int randomListNum = _random.Next (_stairsSuggestList.Count);
            Field[_stairsSuggestList[randomListNum][0], _stairsSuggestList[randomListNum][1]] = (FieldClass.exit);
        }

        return _stairsSuggestList.Count != 0;
    }

    public FieldClass[, ] MakeField (int floorNum, CancellationToken token)
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
            // 実行前にキャンセル確認
            token.ThrowIfCancellationRequested ();

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

        if (!MakeStairs ())
        {
            // 実行前にキャンセル確認
            token.ThrowIfCancellationRequested ();
            // 階段無ければもう一回作り直す
            MakeField (floorNum, token);
        }

        return Field;
    }

    public async UniTask<FieldClass[, ]> MakeFieldAsync (int floorNum, CancellationToken token)
    {
        return await UniTask.Run (() =>
        {
            // 実行前にキャンセル確認
            token.ThrowIfCancellationRequested ();
            return MakeField (floorNum, token);
        });
    }

    public ItemClass[, ] SetItems (int floorNum, CancellationToken token)
    {
        var items = new ItemClass[_width, _height];
        int i, j;

        _floorPosList.Clear ();
        for (i = 0; i < _width; i++)
        {
            // 実行前にキャンセル確認
            token.ThrowIfCancellationRequested ();
            for (j = 0; j < _height; j++)
            {
                if (Field[i, j] == FieldClass.floor)
                {
                    _floorPosList.Add (new int[2] { i, j });
                }
            }
        }

        int randomListNum = 0, randomTmp = 0;

        for (i = 0; i < floorNum; i++)
        {
            while (true)
            {
                // 実行前にキャンセル確認
                token.ThrowIfCancellationRequested ();
                randomTmp = _random.Next (_floorPosList.Count);
                if (randomListNum != randomTmp &&
                 (_floorPosList[randomTmp][0] != _initX && _floorPosList[randomTmp][1] != _initY))
                {// 前の値と違う、プレイヤー位置と異なる
                    break;
                }
            }
            randomListNum = randomTmp;
            if (_floorPosList.Count != 0)
            {
                items[_floorPosList[randomListNum][0], _floorPosList[randomListNum][1]] = ItemClass.potion;
            }
        }

        return items;
    }

    public async UniTask<ItemClass[, ]> SetItemsAsync (int floorNum, CancellationToken token)
    {
        return await UniTask.Run (() =>
        {
            // 実行前にキャンセル確認
            token.ThrowIfCancellationRequested ();
            return SetItems (floorNum, token);
        });
    }

    #endregion // Method
}