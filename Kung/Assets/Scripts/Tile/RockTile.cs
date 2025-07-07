using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RockTile : MonoBehaviour
{
    public Tilemap _miniMapRockTile;

    [SerializeField] private Tilemap _rockTile;

    [HideInInspector] public List<DestroiedTiles> destroiedRockTiles;

    void Start()
    {
        if (destroiedRockTiles != null)
        {
            foreach (DestroiedTiles tile in destroiedRockTiles)
            {
                Vector3Int pos = new Vector3Int(tile.x, tile.y, 0);
                _rockTile.SetTile(pos, null);
                _miniMapRockTile.SetTile(pos, null);
            }
        }
    }
    public void LoadDestroiedTiles(List<DestroiedTiles> LoadDestroiedTiles)
    {
        destroiedRockTiles = LoadDestroiedTiles;
    }

    public void DestroyRockTile(Vector3Int destroyTilePosition)
    {
        Debug.Log($"Explosion at cell {destroyTilePosition}, hasTile? {_rockTile.HasTile(destroyTilePosition)}");
        if (_rockTile.HasTile(destroyTilePosition))
        {
            _rockTile.SetTile(destroyTilePosition, null);
            destroiedRockTiles.Add(new DestroiedTiles(destroyTilePosition.x, destroyTilePosition.y));
        }
        if (_miniMapRockTile != null && _miniMapRockTile.HasTile(destroyTilePosition))
        {
            _miniMapRockTile.SetTile(destroyTilePosition, null);
        }
    }
}
