using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Ore Tile", menuName = "Tiles/Custom Ore Tile")]
public class CustomOreTile : Tile
{
    public enum OreType { None, Coor,Iron, Gold, Diamond,Emurald }
    public OreType oreType;
    public int price;
    // 추가적인 필드...
}