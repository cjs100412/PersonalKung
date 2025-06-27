using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    [SerializeField] private Animator _drillLeft;
    [SerializeField] private Animator _drillRight;
    [SerializeField] private Animator _drillDown;
    

    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down; // 현재 굴착할 방향

    public CinemachineImpulseSource impulseSource; // 카메라 진동관련
    private Coroutine drillCoroutine;

    private float[,] tiles;

    public float shakeInterval = 0.1f;
    private float shakeTimer;

    public float drillDamage;
    public float drillCoolTime;
    
    bool isGround = true;


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
        bool isDrilling = Input.GetKey(KeyCode.X) && isGround;

        // 애니메이션 처리만 따로
        if (isDrilling)
        {
            shakeTimer += Time.deltaTime;

            if (shakeTimer >= shakeInterval)
            {
                impulseSource.GenerateImpulse();
                shakeTimer = 0f;
            }
        }
        else
        {
            shakeTimer = 0f; // 멈췄을 때 타이머 리셋
        }

        // 코루틴 제어는 아래와 같이 유지
        if (Input.GetKeyDown(KeyCode.X) && isGround)
        {
            player.isDrilling = true;

            if (drillCoroutine == null)
                drillCoroutine = StartCoroutine(DrillingRoutine());
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            player.isDrilling = false;

            if (drillCoroutine != null)
            {
                StopCoroutine(drillCoroutine);
                drillCoroutine = null;
                _drillLeft.SetBool("DrillingLeft", false);
                _drillRight.SetBool("DrillingRight", false);
                _drillDown.SetBool("DrillingDown", false);
            }
        }

        //if (Input.GetKey(KeyCode.C))
        //{
        //    Debug.Log(breakableTilemap.WorldToCell(transform.position));
        //}
    }

    private IEnumerator DrillingRoutine()
    {

        while (true)
        {
            Vector3Int currentPos = brokenableTilemap.WorldToCell(transform.position);
            Vector3Int pos = currentPos;

            switch (currentDirectionState)
            {
                case CurrentDirectionState.Left:
                    pos = new Vector3Int(currentPos.x - 1, currentPos.y);
                    _drillLeft.SetBool("DrillingLeft", true);
                    _drillRight.SetBool("DrillingRight", false);
                    _drillDown.SetBool("DrillingDown", false);
                    break;
                case CurrentDirectionState.Right:
                    pos = new Vector3Int(currentPos.x + 1, currentPos.y);
                    _drillRight.SetBool("DrillingRight", true);
                    _drillLeft.SetBool("DrillingLeft", false);
                    _drillDown.SetBool("DrillingDown", false);

                    break;
                case CurrentDirectionState.Down:
                    pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                    _drillDown.SetBool("DrillingDown", true);    
                    _drillLeft.SetBool("DrillingRight", false);    
                    _drillRight.SetBool("DrillingLeft", false);    
                    break;
            }

            (bool valid, int x, int y) = TryCellToIndex(pos);

            if (valid)
            {
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
