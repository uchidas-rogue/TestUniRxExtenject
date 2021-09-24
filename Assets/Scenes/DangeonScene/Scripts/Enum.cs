using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 1,
    upright = 2,
    right = 3,
    downright = 4,
    down = -1,
    downleft = -2,
    left = -3,
    upleft = -4,
    none = -100
}

public enum FieldClass
{
    wall,
    path,
    floor,
    exit,
    roomwallup,
    roomwallright,
    roomwalldown,
    roomwallleft
}

public enum Floor
{
    rocktile
}

public enum Wall
{
    reaf,
    roomup,
    roomupright,
    roomright,
    roomdownright,
    roomdown,
    roomdownleft,
    roomleft,
    roomupleft
}