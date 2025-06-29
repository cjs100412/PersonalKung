using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private Camera mainCamera;
    private HashSet<Vector3Int> removedCells = new HashSet<Vector3Int>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3Int cellPos = mineralTilemap.WorldToCell(collision.transform.position);
        Vector3 tilePos = mineralTilemap.CellToWorld(cellPos) + Vector3.up * 0.3f;

        if (removedCells.Contains(cellPos)) return;

        TileBase tile = mineralTilemap.GetTile(cellPos);

        if (tile is CustomOreTile customTile)
        {
            StartCoroutine(ExprotPrice(customTile.price, tilePos));
            mineralTilemap.SetTile(cellPos, null);
            removedCells.Add(cellPos);
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

