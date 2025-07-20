using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Ore Tile", menuName = "Tiles/Custom Ore Tile")]
public class CustomOreTile : Tile
{
    public enum OreType 
    {
        None,
        Coal,
        Copper,
        Silver,
        Gold,
        Quartz,
        Ruby,
        Topaz,
        Emerald,
        Diamond,
        Rock
    }
    public OreType oreType;
    public int price;
    public int id;
    // �߰����� �ʵ�...
}