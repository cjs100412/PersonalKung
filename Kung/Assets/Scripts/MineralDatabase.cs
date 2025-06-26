using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineralDatabase : MonoBehaviour
{
    [SerializeField] private Tilemap mineralTilemap;
    private HashSet<Vector3Int> removedCells = new HashSet<Vector3Int>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3Int cellPos = mineralTilemap.WorldToCell(collision.transform.position);

        if (removedCells.Contains(cellPos)) return;
        Debug.Log("트리거");
        TileBase tile = mineralTilemap.GetTile(cellPos);
        if (tile is CustomOreTile customTile)
        {
            Debug.Log($"광물 종류: {customTile.oreType}");
            Debug.Log($"광물 가격: {customTile.price}");
            mineralTilemap.SetTile(cellPos, null);
            removedCells.Add(cellPos);
        }
    }

}
