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
    [Header("�巡�� �� ��� ��")]
    [SerializeField] private Tilemap mineralTilemap;
    [SerializeField] private RectTransform _MineralCanvas;

    [Header("Ǯ�� MineralTextPool ����� ����")]
    [SerializeField] private PriceTextPool pool; //text ��� �� Ǯ�� ����� ����

    [Header("�ؽ�Ʈ ���� �ð� ����")]
    [SerializeField] private float _textdissapperTime;
    [SerializeField] private float repeatCycle;

    [SerializeField] private Inventory _inventory;
    UserInventoryService _userInventorySpublvice;

    private Camera mainCamera;
    private HashSet<Vector3Int> removedCells = new HashSet<Vector3Int>();

    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    private void Start()
    {
        mainCamera = Camera.main;

        //for (int count = 1; count <= 24; ++count)
        //{
        //    _inventoryServiceLocator.Service.AcquireItem(1003);
        //}
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
                StartCoroutine(ExprotPrice(customTile.price, tilePos));
                mineralTilemap.SetTile(cellPos, null);
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

