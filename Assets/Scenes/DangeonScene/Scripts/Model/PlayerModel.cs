using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerModel : IPlayerModel
{
    public Vector3ReactiveProperty PlayerInputVec3RP { get; set; } = new Vector3ReactiveProperty ();
    public ReactiveProperty<Direction> DirectionPlayerRP { get; set; } = new ReactiveProperty<Direction> (Direction.none);
    public Vector3ReactiveProperty PlayerPositionVec3RP { get; set; } = new Vector3ReactiveProperty ();
    public Vector3 InitPosVec3 {get;set;} = new Vector3 (49f, 0, 49f);
    public List<ItemClass> ItemList { get; set; } = new List<ItemClass> ();

}