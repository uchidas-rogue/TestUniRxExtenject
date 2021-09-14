using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DangeonFieldModel : IDangeonFieldModel
{
    #region PublicParam
    public IntReactiveProperty FloorNumRP { get; set; } = new IntReactiveProperty (1);
    public BoolReactiveProperty IsFieldSetting { get; set; } = new BoolReactiveProperty (false);
    public int[, , ] Field { get; set; }
    #endregion PublicParam

    #region privateParam
    private int width;
    private int height;
    private Direction direction = Direction.right;
    private int x;
    private int y;
    private int roomWidth;
    private int roomHeiht;
    private int roomEntryX;
    private int roomEntryY;
    private int roomNumber;
    private List<int[]> FloorPosList = new List<int[]> ();
    private List<int[]> StairsSuggestList = new List<int[]> ();

    #endregion privateParam

    #region Method
    // check the position is in Field 
    private bool CheckInside0Position (int x, int y)
    {
        return (x > -1 && x < this.width) &&
            (y > -1 && y < this.height) &&
            (this.Field[x, y, 0] == 0);
    }

    // check the position where can dig 
    private bool CheckCanDig (int x, int y)
    {
        return (CheckInside0Position (x, y + 2 * (int) Direction.up) ||
            CheckInside0Position (x, y + 2 * (int) Direction.down) ||
            CheckInside0Position (x + 2 * (int) Direction.left / Mathf.Abs ((int) Direction.left), y) ||
            CheckInside0Position (x + 2 * (int) Direction.right / Mathf.Abs ((int) Direction.right), y)
        );
    }
    private bool CheckCanDig (int x, int y, Direction direction)
    {
        if (direction == Direction.up || direction == Direction.down)
        {
            return CheckInside0Position (x, y + 2 * ((int) direction / Mathf.Abs ((int) direction)));
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            return CheckInside0Position (x + 2 * ((int) direction / Mathf.Abs ((int) direction)), y);
        }
    }

    private bool CheckCanMakeRoom (int x, int y, int sizeX, int sizeY, int entryX, int entryY)
    {
        return (CheckInside0Position (x - entryX, y) &&
                CheckInside0Position (x - entryX + sizeX - 1, y) &&
                CheckInside0Position (x - entryX, y + (sizeY - 1) * (int) Direction.up) &&
                CheckInside0Position (x - entryX + sizeX - 1, y + (sizeY - 1) * (int) Direction.up)
            ) ||
            (CheckInside0Position (x - entryX, y) &&
                CheckInside0Position (x - entryX + sizeX - 1, y) &&
                CheckInside0Position (x - entryX, y + (sizeY - 1) * (int) Direction.down) &&
                CheckInside0Position (x - entryX + sizeX - 1, y + (sizeY - 1) * (int) Direction.down)
            ) ||
            (CheckInside0Position (x, y - entryY) &&
                CheckInside0Position (x, y - entryY + sizeY - 1) &&
                CheckInside0Position (x + (sizeX - 1) * (int) Direction.left / Mathf.Abs ((int) Direction.left), y - entryY) &&
                CheckInside0Position (x + (sizeX - 1) * (int) Direction.left / Mathf.Abs ((int) Direction.left), y - entryY + sizeY - 1)
            ) ||
            (CheckInside0Position (x, y - entryY) &&
                CheckInside0Position (x, y - entryY + sizeY - 1) &&
                CheckInside0Position (x + (sizeX - 1) * (int) Direction.right / Mathf.Abs ((int) Direction.right), y - entryY) &&
                CheckInside0Position (x + (sizeX - 1) * (int) Direction.right / Mathf.Abs ((int) Direction.right), y - entryY + sizeY - 1)
            );
    }

    private bool CheckCanMakeRoom (int x, int y, int sizeX, int sizeY, int entryX, int entryY, Direction direction)
    {
        if (direction == Direction.up || direction == Direction.down)
        {
            return (CheckInside0Position (x - entryX, y) &&
                CheckInside0Position (x - entryX + sizeX - 1, y) &&
                CheckInside0Position (x - entryX, y + (sizeY - 1) * (int) direction) &&
                CheckInside0Position (x - entryX + sizeX - 1, y + (sizeY - 1) * (int) direction)
            );
        }
        else //(direction == Direction.left || direction == Direction.right)
        {
            return (CheckInside0Position (x, y - entryY) &&
                CheckInside0Position (x, y - entryY + sizeY - 1) &&
                CheckInside0Position (x + (sizeX - 1) * ((int) direction / Mathf.Abs ((int) direction)), y - entryY) &&
                CheckInside0Position (x + (sizeX - 1) * ((int) direction / Mathf.Abs ((int) direction)), y - entryY + sizeY - 1)
            );
        }
    }

    private void CheckFloorPosition ()
    {
        FloorPosList.Clear ();
        for (int i = 0; i < (this.width - 1) / 2; i++)
        {
            for (int j = 0; j < (this.height - 1) / 2; j++)
            {
                if (Field[2 * i + 1, 2 * j + 1, 0] != 0)
                {
                    FloorPosList.Add (new int[2] { 2 * i + 1, 2 * j + 1 });
                }
            }
        }
    }

    private void CheckStairsSuggestPosition ()
    {
        CheckFloorPosition ();
        StairsSuggestList.Clear ();
        foreach (var item in FloorPosList)
        {
            if ((Field[item[0] + 1, item[1], 0] + Field[item[0] - 1, item[1], 0] +
                    Field[item[0], item[1] + 1, 0] + Field[item[0], item[1] - 1, 0]) == 1)
            {
                StairsSuggestList.Add (item);
            }
        }
    }

    private bool CheckAnyDigPosition ()
    {
        for (int i = 0; i < (this.width - 1) / 2; i++)
        {
            for (int j = 0; j < (this.height - 1) / 2; j++)
            {
                if (CheckCanDig (2 * i + 1, 2 * j + 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ChangeDir ()
    {
        switch (Random.Range (0, 4))
        {
            case 0:
                this.direction = Direction.up;
                break;
            case 1:
                this.direction = Direction.down;
                break;
            case 2:
                this.direction = Direction.left;
                break;
            case 3:
                this.direction = Direction.right;
                break;
        }
    }

    private void ChangePosition ()
    {
        CheckFloorPosition ();
        int randomListNum = Random.Range (0, FloorPosList.Count);
        if (FloorPosList.Count != 0)
        {
            this.x = FloorPosList[randomListNum][0];
            this.y = FloorPosList[randomListNum][1];
        }
    }

    private void ChangeroomSize ()
    {
        this.roomWidth = Random.Range (4, 8);
        if (this.roomWidth % 2 == 0)
        { //odd num
            this.roomWidth++;
        }
        this.roomHeiht = Random.Range (4, 8);
        if (this.roomHeiht % 2 == 0)
        { //odd num
            this.roomHeiht++;
        }
    }

    private void ChangeroomEntry ()
    {
        this.roomEntryX = Random.Range (0, this.roomWidth + 1);
        if (this.roomEntryX % 2 == 1)
        { //even num
            this.roomEntryX--;
        }
        this.roomEntryY = Random.Range (0, this.roomHeiht + 1);
        if (this.roomEntryY % 2 == 1)
        { //even num
            this.roomEntryY--;
        }
    }

    private void MakeRoom ()
    {
        if (direction == Direction.up || direction == Direction.down)
        {
            for (int i = 0; i < roomWidth; i++)
            {
                for (int j = 0; j < roomHeiht; j++)
                {
                    //(Field[x-entry,y]),(Field[x-entry,y+(size-1)])
                    //(Field[x-entry+size-1,y]),(Field[x-entry+size-1,y+(size-1)])
                    Field[this.x - roomEntryX + i, this.y + j * (int) direction, 0] = (int) FieldClass.floor;
                }
            }
            roomNumber++;
        }
        else //if (direction == Direction.left || direction == Direction.right)
        {
            for (int i = 0; i < roomWidth; i++)
            {
                for (int j = 0; j < roomHeiht; j++)
                {
                    //(Field[x,y-entry]),(Field[x+(size-1),y-entry])
                    //(Field[x,y-entry+size-1]),(Field[x+(size-1),y-entry+size-1])
                    Field[this.x + i * ((int) direction / Mathf.Abs ((int) direction)), this.y - roomEntryY + j, 0] = (int) FieldClass.floor;
                }
            }
            roomNumber++;
        }
    }

    private void MakePath ()
    {
        if (direction == Direction.up || direction == Direction.down)
        {
            this.y += (int) direction;
            Field[this.x, this.y, 0] = ((int) FieldClass.path);
            this.y += (int) direction;
            Field[this.x, this.y, 0] = ((int) FieldClass.path);
        }
        else // if (direction == Direction.left || direction == Direction.right)
        {
            this.x += ((int) direction / Mathf.Abs ((int) direction));
            Field[this.x, this.y, 0] = ((int) FieldClass.path);
            this.x += ((int) direction / Mathf.Abs ((int) direction));
            Field[this.x, this.y, 0] = ((int) FieldClass.path);
        }
    }

    private void MakeStairs ()
    {
        CheckStairsSuggestPosition ();
        if (StairsSuggestList.Count != 0)
        {
            int randomListNum = Random.Range (0, StairsSuggestList.Count);
            Field[StairsSuggestList[randomListNum][0], StairsSuggestList[randomListNum][1], 0] = ((int) FieldClass.exit);
        }
    }
    #endregion Method

    public void MakeField (int width, int height, int level)
    {
        x = 49;
        y = 49;
        this.width = width;
        this.height = height;
        Field = new int[width, height, 2];

        if (CheckCanMakeRoom (x, y, roomWidth, roomHeiht, roomEntryX, roomEntryY))
        {
            while (!CheckCanMakeRoom (x, y, roomWidth, roomHeiht, roomEntryX, roomEntryY, direction))
            {
                ChangeDir ();
            }
            MakeRoom ();
        }

        int cnt = 0;

        while (cnt < (x + y) * level)
        {
            ChangeDir ();
            ChangeroomSize ();
            ChangeroomEntry ();

            if (CheckCanMakeRoom (x, y, roomWidth, roomHeiht, roomEntryX, roomEntryY, direction))
            {
                MakeRoom ();
            }
            else if (CheckCanDig (x, y, direction))
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
    }
}