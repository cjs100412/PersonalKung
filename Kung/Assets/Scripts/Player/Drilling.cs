using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum CurrentDirectionState
{
    None,
    Left,
    Right,
    Down
}
public class Drilling : MonoBehaviour
{
    [Header("잘 연결해야 함")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Tilemap brokenableTilemap;
    [SerializeField] private Tilemap mineralTilemap;

    [Header("부셔지는 타일맵 스프라이트 배열")]
    public Sprite[] brokenTileSprites;

    [Header("드릴 성능")]
    public float drillDamage;
    public float drillCoolTime; // 낮을수록 좋음

    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down; // 현재 굴착할 방향

    public bool isDrilling = false;

    private int _width;
    private int _height;
    private int _offsetX;
    private int _offsetY;
    private int _spriteIndex;

    private float[,] tiles; 


    

    private void Start()
    {
        tileArrayInit();
    }


    /// <summary>
    /// 타일맵의 타일 하나하나 초기화
    /// </summary>
    private void tileArrayInit()
    {
        BoundsInt bounds = brokenableTilemap.cellBounds;
        _width = bounds.xMax - bounds.xMin;
        _height = bounds.yMax - bounds.yMin;
        tiles = new float[_width, _height];
        _offsetX = -bounds.xMin;
        _offsetY = -bounds.yMin;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                tiles[TryCellToIndex(pos).x, TryCellToIndex(pos).y] = 100;
            }
        }
        _spriteIndex = 100 / brokenTileSprites.Length;
    }


    /// <summary>
    /// 들어온 Vector3Int를 음수가 나오지 않도록 오프셋으로 조절해서 배열에서 사용할 인덱스 반환
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns>배열 범위 안의 좌표인지, 2차원 배열에서 사용할 x,y</returns>
    private (bool valid, int x, int y) TryCellToIndex(Vector3Int cellPos)
    {
        int x = cellPos.x + _offsetX;
        int y = cellPos.y + _offsetY;
        if (x < 0 || y < 0 || x >= _width || y >= _height)
            return (false, 0, 0);

        return (true, x, y);
    }


    /// <summary>
    /// 드릴 키를 눌렀을 때 동작할 코루틴
    /// </summary>
    /// <returns>드릴 쿨타임만큼 기다림</returns>
    public IEnumerator DrillingRoutine()
    {

        while (true)
        {
            Vector3Int currentPos = brokenableTilemap.WorldToCell(transform.position);
            Vector3Int pos = currentPos;
            switch (currentDirectionState)
            {
                case CurrentDirectionState.Left:
                    pos = new Vector3Int(currentPos.x - 1, currentPos.y);
                    break;
                case CurrentDirectionState.Right:
                    pos = new Vector3Int(currentPos.x + 1, currentPos.y);

                    break;
                case CurrentDirectionState.Down:
                    pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                    break;
            }

            (bool valid, int x, int y) = TryCellToIndex(pos);
            if (brokenableTilemap.GetTile(pos) == null)
            {
                isDrilling = false;

            }
            else if (valid)
            {
                isDrilling = true;

                tiles[x, y] -= drillDamage;
                if (brokenableTilemap.GetTile(pos) != null)
                {
                    if (tiles[x, y] <= 0)
                    {
                        brokenableTilemap.SetTile(pos, null);
                    }
                    else
                    {
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        int index = Mathf.Clamp((int)(tiles[x, y] / _spriteIndex), 0, brokenTileSprites.Length - 1);
                        newTile.sprite = brokenTileSprites[index];
                        brokenableTilemap.SetTile(pos, newTile);
                    }
                }
                
            }
            

                yield return new WaitForSeconds(drillCoolTime);
        }
    }

}
