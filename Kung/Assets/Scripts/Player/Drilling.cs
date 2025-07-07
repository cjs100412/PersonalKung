using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [Header("�� �����ؾ� ��")]
    [SerializeField] private Tilemap _mineralTilemap;
    [Header("�̴ϸ� ����")]
    public Tilemap _miniMapFrontTilemap;   
    [SerializeField] private TextMeshProUGUI _depthText;    
    private int _surfaceY; 

    [Header("�μ����� Ÿ�ϸ� ��������Ʈ �迭")]
    public Sprite[] brokenTileSprites;

    [Header("�帱 ����")]
    [SerializeField] PlayerStats _playerState;
    //public float drillDamage;
    public float drillCoolTime; // �������� ����

    private PlayerMovement _player;
    public Tilemap _brokenableTilemap;
    public Tilemap _rockTilemap;

    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down; // ���� ������ ����

    public bool isDrilling = false;

    private int _width;
    private int _height;
    private int _offsetX;
    private int _offsetY;
    private int _spriteIndex;

    private float[,] _tiles; 


    

    private void Start()
    {
        _player = GetComponent<PlayerMovement>();
        tileArrayInit();
    }

    private void Update()
    {
        //�̴��� �ڵ� ��ġ�� �ּ� Ǯ ��
        Vector3Int currentCell = _brokenableTilemap.WorldToCell(transform.position); 
        int depth = Mathf.Max(0, _surfaceY - currentCell.y);    
        _depthText.text = depth + "m";
    }

    /// <summary>
    /// Ÿ�ϸ��� Ÿ�� �ϳ��ϳ� �ʱ�ȭ
    /// </summary>
    private void tileArrayInit()
    {
        BoundsInt bounds = _brokenableTilemap.cellBounds;
        _width = bounds.xMax - bounds.xMin;
        _height = bounds.yMax - bounds.yMin;
        _tiles = new float[_width, _height];
        _offsetX = -bounds.xMin;
        _offsetY = -bounds.yMin;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                _tiles[TryCellToIndex(pos).x, TryCellToIndex(pos).y] = 30;
            }
        }
        _spriteIndex = 30 / brokenTileSprites.Length;
    }


    /// <summary>
    /// ���� Vector3Int�� ������ ������ �ʵ��� ���������� �����ؼ� �迭���� ����� �ε��� ��ȯ
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns>�迭 ���� ���� ��ǥ����, 2���� �迭���� ����� x,y</returns>
    private (bool valid, int x, int y) TryCellToIndex(Vector3Int cellPos)
    {
        int x = cellPos.x + _offsetX;
        int y = cellPos.y + _offsetY;
        if (x < 0 || y < 0 || x >= _width || y >= _height)
            return (false, 0, 0);

        return (true, x, y);
    }


    /// <summary>
    /// �帱 Ű�� ������ �� ������ �ڷ�ƾ
    /// </summary>
    /// <returns>�帱 ��Ÿ�Ӹ�ŭ ��ٸ�</returns>
    public IEnumerator DrillingRoutine()
    {

        while (true)
        {
            Vector3Int currentPos = _brokenableTilemap.WorldToCell(transform.position);
            Vector3Int rockCurrentPos = _rockTilemap.WorldToCell(transform.position);
            Vector3Int pos = currentPos;
            Vector3Int rockPos = rockCurrentPos;
            switch (currentDirectionState)
            {
                case CurrentDirectionState.Left:
                    pos = new Vector3Int(currentPos.x - 1, currentPos.y);
                    rockPos = new Vector3Int(rockCurrentPos.x - 1, rockCurrentPos.y);
                    break;
                case CurrentDirectionState.Right:
                    pos = new Vector3Int(currentPos.x + 1, currentPos.y);
                    rockPos = new Vector3Int(rockCurrentPos.x + 1, rockCurrentPos.y);
                    break;
                case CurrentDirectionState.Down:
                    pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                    rockPos = new Vector3Int(rockCurrentPos.x, rockCurrentPos.y - 1);
                    break;
            }

            (bool valid, int x, int y) = TryCellToIndex(pos);
            if (!_brokenableTilemap.HasTile(pos) && !_rockTilemap.HasTile(rockPos))
            {
                isDrilling = false;

            }
            else if(_rockTilemap.HasTile(rockPos))
            {
                isDrilling = true;
            }
            else
            {
                isDrilling = true;

                _tiles[x, y] -= _playerState.drillDamage;
                if (_brokenableTilemap.GetTile(pos) != null)
                {
                    if (_tiles[x, y] <= 0)
                    {
                        _brokenableTilemap.SetTile(pos, null);
                        if (_miniMapFrontTilemap != null && _miniMapFrontTilemap.HasTile(pos))    
                        {
                            _miniMapFrontTilemap.SetTile(pos, null); 
                        }
                        isDrilling = false;
                        yield return new WaitForSeconds(drillCoolTime);
                    }
                    else
                    {
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        int index = Mathf.Clamp((int)(_tiles[x, y] / _spriteIndex), 0, brokenTileSprites.Length - 1);
                        newTile.sprite = brokenTileSprites[index];
                        _brokenableTilemap.SetTile(pos, newTile);
                    }
                }

            }
            

                yield return new WaitForSeconds(drillCoolTime);
        }
    }

}
