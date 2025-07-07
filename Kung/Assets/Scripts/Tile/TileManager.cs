using System.Buffers.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TileManager : MonoBehaviour
{
    [SerializeField] Transform par;
    [Header("미리 만든 타일맵 연결")]
    [SerializeField] private GameObject brokenTileMap;
    [SerializeField] private GameObject backGroundTIleMap;
    [SerializeField] private GameObject frontMiniMapTilemap;
    [SerializeField] private GameObject backMiniMapTilemap;
    [SerializeField] private Tile mineralTile;
    
    [SerializeField] private GameObject Level1;
    [SerializeField] private GameObject Level2;
    
    [SerializeField] private GameObject miniMapRockTile;

    [SerializeField] private Drilling _drilling;

    public Tilemap brokenableTilemap;
    public Tilemap miniMapFrontTilemap;
    
    public Sprite[] brokenTileSprites;
    [SerializeField] private Tile mineralTile;
    [SerializeField] private DynamiteBOOM _boom;
    
    private Tilemap brokenTileMapInstance;

    private int _width;
    private int _height;
    private int _offsetX;
    private int _offsetY;

    float firstThreshold;
    float secondThreshold;
    public float[,] tiles;
    public int baseHp;


    void Awake()
    {
        Instantiate(backGroundTIleMap, par);
        Instantiate(backMiniMapTilemap, par);
        brokenableTilemap = Instantiate(brokenTileMap, par).GetComponent<Tilemap>();
        miniMapFrontTilemap = Instantiate(frontMiniMapTilemap, par).GetComponent<Tilemap>();
        tileArrayInit();
        GameObject lv1 = Instantiate(Level1, new Vector2(0, firstThreshold * 0.45f), Quaternion.identity);    
        GameObject lv2 = Instantiate(Level2, new Vector2(0, firstThreshold * 0.75f), Quaternion.identity);
        lv1.GetComponent<Transform>().localScale = new Vector2(_width, _height / 3) * 0.3f; 
        lv2.GetComponent<Transform>().localScale = new Vector2(_width, _height / 3) * 0.3f;

        _drilling._brokenableTilemap = Instantiate(brokenTileMap, par).GetComponent<Tilemap>();
        _drilling._miniMapFrontTilemap = Instantiate(frontMiniMapTilemap, par).GetComponent<Tilemap>();
        _boom._miniMapRockTile = Instantiate(miniMapRockTile, par).GetComponent<Tilemap>();
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

        firstThreshold = bounds.yMax - (_height / 3f);      
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
        if (y < firstThreshold && y >= secondThreshold) return baseHp * 2;
        if (y < secondThreshold) return baseHp * 3;
        return baseHp;
    }



}
