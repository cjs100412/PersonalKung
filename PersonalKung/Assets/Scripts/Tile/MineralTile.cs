using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class MineralTile : MonoBehaviour
{
    [Header("드래그 앤 드롭 해")]
    [SerializeField] private Tilemap mineralTilemap;
    [SerializeField] private RectTransform _MineralCanvas;

    [Header("풀링 MineralTextPool 끌어다 놓기")]
    [SerializeField] private PriceTextPool pool; //text 출력 시 풀링 사용을 위함

    [Header("텍스트 관련 시간 설정")]
    [SerializeField] private float _textdissapperTime;
    [SerializeField] private float repeatCycle;

    [SerializeField] private Inventory _inventory;
    UserInventoryService _userInventorySpublvice;

    private Camera mainCamera;
    private HashSet<Vector3Int> removedCells = new HashSet<Vector3Int>();

    public HUD ui;

    [HideInInspector] public List<DestroiedTiles> destroiedMineralTiles;

    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    private void Start()
    {
        mainCamera = Camera.main;
        if (destroiedMineralTiles != null)
        {
            foreach (DestroiedTiles tile in destroiedMineralTiles)
            {
                Vector3Int pos = new Vector3Int(tile.x, tile.y, 0);
                mineralTilemap.SetTile(pos, null);
            }
        }
        //for (int count = 1; count <= 24; ++count)
        //{
        //    _inventoryServiceLocator.Service.AcquireItem(1003);
        //}
    }

    public void LoadDestroiedTiles(List<DestroiedTiles> LoadDestroiedTiles)
    {
        destroiedMineralTiles = LoadDestroiedTiles;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (_inventoryServiceLocator.Service.Items.Count >= 25)
        //{
        //    return;
        //}


        Vector3Int cellPos = mineralTilemap.WorldToCell(collision.transform.position);
        Vector3 tilePos = mineralTilemap.CellToWorld(cellPos) + Vector3.up * 0.3f;

        if (removedCells.Contains(cellPos)) return;

        TileBase tile = mineralTilemap.GetTile(cellPos);

        if (tile is CustomOreTile customTile)
        {
            if (_inventoryServiceLocator.Service.CanAcquireItem(customTile.id))
            {
                _inventoryServiceLocator.Service.AcquireItem(customTile.id);
                SoundManager.Instance.PlaySFX(SFX.Mineral);
                StartCoroutine(ExprotPrice(customTile.price, tilePos));
                ui.score += customTile.price;
                mineralTilemap.SetTile(cellPos, null);
                destroiedMineralTiles.Add(new DestroiedTiles(cellPos.x, cellPos.y));
                removedCells.Add(cellPos);
            }
            
        }
    }

    IEnumerator ExprotPrice(int price, Vector3 pos)
    {
        float time = _textdissapperTime;

        GameObject go = pool.Get();
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        RectTransform rect = go.GetComponent<RectTransform>();

        Vector3 screenPos = mainCamera.WorldToScreenPoint(pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _MineralCanvas,
            screenPos,
            null,
            out Vector2 localPos
        );

        Vector2 basePos = localPos;
        Color originalColor = text.color;
        float offsetY = 0f;

        text.text = "+" + price.ToString();

        while (time >= 0)
        {
            time -= repeatCycle;
            offsetY += 5f;

            rect.anchoredPosition = basePos + new Vector2(0, offsetY);

            float alpha = time / _textdissapperTime;
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return new WaitForSeconds(repeatCycle);
        }

        text.color = originalColor;
        pool.Return(go);
    }

}

