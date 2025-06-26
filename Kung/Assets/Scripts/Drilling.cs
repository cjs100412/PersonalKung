using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject drill;
    [SerializeField] private Tilemap brokenableTilemap;
    [SerializeField] private Tilemap mineralTilemap;

    public float drillDamage;
    public float drillCoolTime;

    bool isGround = true;

    // 현재 굴착할 방향
    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down;

    //private Dictionary<Vector3Int,float> tileDict = new Dictionary<Vector3Int,float>();
    private float[,] tiles;

    [SerializeField] private Animator drillAnimator;
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

    private Coroutine drillCoroutine;

    // Update is called once per frame
    private void Update()
    {
        bool isDrilling = Input.GetKey(KeyCode.X) && isGround;

        // 애니메이션 처리만 따로
        drillAnimator.SetBool("Drilling", isDrilling);

        // 코루틴 제어는 아래와 같이 유지
        if (Input.GetKeyDown(KeyCode.X) && isGround)
        {
            if (drillCoroutine == null)
                drillCoroutine = StartCoroutine(DrillingRoutine());
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (drillCoroutine != null)
            {
                StopCoroutine(drillCoroutine);
                drillCoroutine = null;
            }
        }

        //if (Input.GetKey(KeyCode.C))
        //{
        //    Debug.Log(breakableTilemap.WorldToCell(transform.position));
        //}
    }

    private IEnumerator DrillingRoutine()
    {
        drillAnimator.SetBool("Drilling", true);

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

            var (valid, x, y) = TryCellToIndex(pos);
            if (valid)
            {
                tiles[x, y] -= drillDamage;

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

            yield return new WaitForSeconds(drillCoolTime);
        }
    }

}
