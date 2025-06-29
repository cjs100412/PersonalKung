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
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Tilemap brokenableTilemap;
    [SerializeField] private Tilemap mineralTilemap;
    

    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down; // 현재 굴착할 방향

    //public CinemachineImpulseSource impulseSource; // 카메라 진동관련
    //private Coroutine drillCoroutine;

    private float[,] tiles;

    public float shakeInterval = 0.1f;
    private float shakeTimer;

    public float drillDamage;
    public float drillCoolTime;
    
    bool isGround = true;
    public bool isDrilling = false;

    public Sprite[] brokenTileSprites;

    private int spriteIndex;

    int width;
    int height;

    int offsetX;
    int offsetY;


    private (bool valid, int x, int y) TryCellToIndex(Vector3Int cellPos)
    {
        int x = cellPos.x + offsetX;
        int y = cellPos.y + offsetY;
        if (x < 0 || y < 0 || x >= width || y >= height)
            return (false, 0, 0);

        return (true, x, y);
    }

    private void Start()
    {
        BoundsInt bounds = brokenableTilemap.cellBounds;
        width = bounds.xMax - bounds.xMin;
        height = bounds.yMax - bounds.yMin;
        tiles = new float[width, height];
        offsetX = -bounds.xMin;
        offsetY = -bounds.yMin;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                //tileDict[pos] = 100;
                tiles[TryCellToIndex(pos).x, TryCellToIndex(pos).y] = 100; 
            }
        }
        spriteIndex = 100 / brokenTileSprites.Length;
    }


    private void Update()
    {
        

        // 코루틴 제어는 아래와 같이 유지
       

        //if (Input.GetKey(KeyCode.C))
        //{
        //    Debug.Log(breakableTilemap.WorldToCell(transform.position));
        //}
    }

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
                        int index = Mathf.Clamp((int)(tiles[x, y] / spriteIndex), 0, brokenTileSprites.Length - 1);
                        newTile.sprite = brokenTileSprites[index];
                        brokenableTilemap.SetTile(pos, newTile);
                    }
                }
                
            }
            

                yield return new WaitForSeconds(drillCoolTime);
        }
    }

}
