using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IMiniMapStringService
{
    string MakeMiniMapString (int playerposx, int playerposy,int[,,] field, bool isPickup = false);
}

public class MiniMapStringService : IMiniMapStringService
{
    public MiniMapStringService ()
    {
        _mapStringBuilder = new StringBuilder ();
    }

    private int _cntX;
    private int _cntY;

    private StringBuilder _mapStringBuilder;

    public string MakeMiniMapString (int playerposx, int playerposy, int[,,] field, bool isPickup = false)
    {
        if (field == null){ return ""; }

        _mapStringBuilder.Clear ();

        if (isPickup)
        {
            for (_cntY = field.GetLength (0) - 1; _cntY >= 0; _cntY--)
            {
                for (_cntX = 0; _cntX < field.GetLength (1); _cntX++)
                {
                    ConvObjtoRichtext (playerposx, playerposy, _cntX, _cntY, field);
                }
                _mapStringBuilder.AppendLine ("");
            }
        }
        else
        {
            for (_cntY = playerposy + 7; _cntY >= playerposy - 7; _cntY--)
            {
                for (_cntX = playerposx - 10; _cntX <= playerposx + 10; _cntX++)
                {
                    ConvObjtoRichtext (playerposx, playerposy, _cntX, _cntY, field);
                }
                _mapStringBuilder.AppendLine ("");
            }
        }

        return _mapStringBuilder.ToString ();
    }

    /// <summary>
    /// それぞれのオブジェクトをリッチテキストに置き換える
    /// </summary>
    /// <param name="playerposx"></param>
    /// <param name="playerposy"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ConvObjtoRichtext (int playerposx, int playerposy, int x, int y, int[,,] field)
    {
        if (x == playerposx && y == playerposy)
        { //player position
            _mapStringBuilder.Append ("<color=yellow>●</color>");
        }
        else if (field[x, y, 0] == 3)
        { //exit position
            if (field[x, y, 1] == 1)
            {
                _mapStringBuilder.Append ("<color=green>■</color>");
            }
            else
            {
                _mapStringBuilder.Append ("   ");
            }
        }
        else if (field[x, y, 0] == 1 || field[x, y, 0] == 2)
        { //floor position
            if (field[x, y, 1] == 1)
            {
                _mapStringBuilder.Append ("<color=blue>■</color>");
            }
            else
            {
                _mapStringBuilder.Append ("   ");
            }
        }
        else
        { //wall position
            if (field[x, y, 1] == 1)
            {
                _mapStringBuilder.Append ("■");
            }
            else
            {
                _mapStringBuilder.Append ("   ");
            }
        }
    }
}