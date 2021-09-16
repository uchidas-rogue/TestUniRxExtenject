using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DangeonFieldModel : IDangeonFieldModel
{
    #region PublicParam
    public IntReactiveProperty FloorNumRP { get; set; } = new IntReactiveProperty (1);
    public bool IsFieldSetting { get; set; } = false;
    public int[, , ] Field { get; set; }
    #endregion PublicParam

    public void MakeField (int width, int height, int level)
    {
        var makeFieldSevice = new MakeFieldService (width, height, 49, 49);

        if (makeFieldSevice.CheckCanMakeRoom ())
        {
            while (!makeFieldSevice.CheckCanMakeRoomDirection ())
            {
                makeFieldSevice.ChangeDir ();
            }
            makeFieldSevice.MakeRoom ();
        }

        int cnt = 0;

        while (cnt < (width + height) * level)
        {
            makeFieldSevice.ChangeDir ();
            makeFieldSevice.ChangeroomSize ();
            makeFieldSevice.ChangeroomEntry ();

            if (makeFieldSevice.CheckCanMakeRoomDirection ())
            {
                makeFieldSevice.MakeRoom ();
            }
            else if (makeFieldSevice.CheckCanDigDirection ())
            {
                makeFieldSevice.MakePath ();
            }
            else
            {
                makeFieldSevice.ChangePosition ();
            }
            cnt++;
        }

        makeFieldSevice.MakeStairs ();

        Field = makeFieldSevice.Field;
        // release
        makeFieldSevice = null;

    }
}