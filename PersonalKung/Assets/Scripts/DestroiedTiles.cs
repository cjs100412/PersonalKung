using System;
using UnityEngine;

[Serializable]
public struct DestroiedTiles
{
    public int x;
    public int y;
    public DestroiedTiles(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
