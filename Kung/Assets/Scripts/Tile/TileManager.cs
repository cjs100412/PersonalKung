using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] Transform par;
    [SerializeField] Transform _playerTransform;
    [Header("미리 만든 타일맵 연결")]
    [SerializeField] private GameObject brokenTileMap;
    [SerializeField] private GameObject backGroundTIleMap;
    [SerializeField] private GameObject frontMiniMapTilemap;
    [SerializeField] private GameObject backMiniMapTilemap;
    [SerializeField] private Tile mineralTile;
    
    [SerializeField] private GameObject[] Levels;
    
    [SerializeField] private GameObject miniMapRockTile;

    [SerializeField] private Drilling _drilling;

    [HideInInspector] public Tilemap brokenableTilemap;
    [HideInInspector] public Tilemap miniMapFrontTilemap;
    
    public Sprite[] brokenTileSprites;
    [SerializeField] private RockTile _rockTile;
    
    private Tilemap brokenTileMapInstance;

    private int _width;
    private int _height;
    private int _offsetX;
    private int _offsetY;

    float firstThreshold;
    float secondThreshold;
    public float[,] tiles;
    public int baseHp;
    public HUD ui;

    private float[] tileLevel = new float[5];
    [HideInInspector] public List<DestroiedTiles> destroiedTiles;
    void Awake()
    {
        Instantiate(backGroundTIleMap, par);
        Instantiate(backMiniMapTilemap, par);
        brokenableTilemap = Instantiate(brokenTileMap, par).GetComponent<Tilemap>();
        miniMapFrontTilemap = Instantiate(frontMiniMapTilemap, par).GetComponent<Tilemap>();
        tileArrayInit();
        InitLevel();
        _rockTile._miniMapRockTile = Instantiate(miniMapRockTile, par).GetComponent<Tilemap>();
    }

    public void LoadDestroiedTiles(List<DestroiedTiles> LoadDestroiedTiles)
    {
        destroiedTiles = LoadDestroiedTiles;
        if (destroiedTiles != null)
        {
            foreach (DestroiedTiles tile in destroiedTiles)
            {
                Vector3Int pos = new Vector3Int(tile.x, tile.y, 0);
                brokenableTilemap.SetTile(pos, null);
                miniMapFrontTilemap.SetTile(pos, null);
            }
        }
    }

    private void InitLevel()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            GameObject go = Instantiate(Levels[i], new Vector2(0, firstThreshold * (0.15f + (0.3f * (i + 1)))), Quaternion.identity);
            go.GetComponent<Transform>().localScale = new Vector2(_width, _height / 6) * 0.3f;

        }
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

        firstThreshold = bounds.yMax - (_height / 6f);
        for (int i = 0; i < 5; i++)
        {
            tileLevel[i] = bounds.yMax - (_height * (i + 1)/ 6f);
        }
        secondThreshold = bounds.yMax - (_height * 2f / 3f); 
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (brokenableTilemap.HasTile(pos))
                {
                    tiles[TryCellToIndex(pos).x, TryCellToIndex(pos).y] = GetTileMaxHp(y);
                }
            }
        }
    }

    public void DamageTile(Vector3Int target, float damage)
    {
        (int x, int y) = TryCellToIndex(target);
        tiles[x, y] -= damage;
        Debug.Log($"{x} , {y} : 체력 {tiles[x, y]}");

        if (tiles[x, y] <= 0)
        {
            brokenableTilemap.SetTile(target, null);
            miniMapFrontTilemap.SetTile(target, null);
            destroiedTiles.Add(new DestroiedTiles(target.x, target.y));

            Vector3Int currentCell = brokenableTilemap.WorldToCell(_playerTransform.position);
            int depth = Mathf.Max(0, 0 - currentCell.y);
            ui.score += depth;
            return;
        }

        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        int hp = GetTileMaxHp(target.y);
        
        int index = Mathf.Clamp((int)(tiles[x, y] / (hp / brokenTileSprites.Length)), 0, brokenTileSprites.Length - 1);
        newTile.sprite = brokenTileSprites[index];
        brokenableTilemap.SetTile(target, newTile);
        
        
    }

    /// <summary>
    /// 들어온 Vector3Int를 음수가 나오지 않도록 오프셋으로 조절해서 배열에서 사용할 인덱스 반환
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns>배열 범위 안의 좌표인지, 2차원 배열에서 사용할 x,y</returns>
    public (int x, int y) TryCellToIndex(Vector3Int cellPos)
    {
        int x = cellPos.x + _offsetX;
        int y = cellPos.y + _offsetY;
        if (x < 0 || y < 0 || x >= _width || y >= _height)
            throw new System.Exception("범위 초과");

        return (x, y);
    }
    private int GetTileMaxHp(int y)
    {
        if (y >= firstThreshold)
        {
            return baseHp;
        }
        for (int i = 1; i < tileLevel.Length; i++)
        {
            if (y < tileLevel[i - 1] && y >= tileLevel[i])
            {
                return (int)(baseHp * (i + 0.5f));
            }
        }
        return (int)(baseHp * (tileLevel.Length + 0.5));
    }



}
