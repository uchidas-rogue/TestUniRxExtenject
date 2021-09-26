using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;

public interface IMapStringService
{
    string MakeMapString (int playerposx, int playerposy, bool isPickup = false);
}

public class MapStringService : IMapStringService
{
    IDangeonFieldModel _dangeonFieldModel;

    public MapStringService (IDangeonFieldModel dfm)
    {
        _mapStringBuilder = new StringBuilder ();
        _dangeonFieldModel = dfm;
    }

    int _cntX;
    int _cntY;
    StringBuilder _mapStringBuilder;

    public string MakeMapString (int playerposx, int playerposy, bool isPickup = false)
    {
        if (_dangeonFieldModel.Field == null) { return ""; }

        _mapStringBuilder.Clear ();

        if (isPickup)
        {
            for (_cntY = _dangeonFieldModel.Field.GetLength (0) - 1; _cntY >= 0; _cntY--)
            {
                for (_cntX = 0; _cntX < _dangeonFieldModel.Field.GetLength (1); _cntX++)
                {
                    ConvObjtoRichtext (playerposx, playerposy, _cntX, _cntY);
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
                    ConvObjtoRichtext (playerposx, playerposy, _cntX, _cntY);
                }
                _mapStringBuilder.AppendLine ("");
            }
        }

        return _mapStringBuilder.ToString ();
    }

    /// <summary>
    /// 指定の位置がFieldの内側かどうか
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool CheckInsidePosition (int x, int y)
    {
        return (x > -1 && x < _dangeonFieldModel.Field.GetLength (0)) &&
            (y > -1 && y < _dangeonFieldModel.Field.GetLength (1));
    }

    /// <summary>
    /// それぞれのオブジェクトをリッチテキストに置き換える
    /// </summary>
    /// <param name="playerposx"></param>
    /// <param name="playerposy"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void ConvObjtoRichtext (int playerposx, int playerposy, int x, int y)
    {
        if (!CheckInsidePosition (_cntX, _cntY)) { return; }

        if (x == playerposx && y == playerposy)
        { //player position
            _mapStringBuilder.Append ("<color=yellow>●</color>");
        }
        else if (_dangeonFieldModel.Field[x, y] == FieldClass.exit)
        { //exit position
            if (_dangeonFieldModel.Map[x, y] == MapClass.walked)
            {
                _mapStringBuilder.Append ("<color=green>■</color>");
            }
            else
            {
                _mapStringBuilder.Append ("   ");
            }
        }
        else if (_dangeonFieldModel.Field[x, y] == FieldClass.path ||
            _dangeonFieldModel.Field[x, y] == FieldClass.floor)
        { //floor position
            if (_dangeonFieldModel.Map[x, y] == MapClass.walked)
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
            if (_dangeonFieldModel.Map[x, y] == MapClass.walked)
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