using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] Transform par;
    [Header("미리 만든 타일맵 연결")]
    [SerializeField] private GameObject brokenTileMap;
    [SerializeField] private GameObject backGroundTIleMap;
    [SerializeField] private GameObject frontMiniMapTilemap;
    [SerializeField] private GameObject backMiniMapTilemap;
    [SerializeField] private GameObject miniMapRockTile;

    [SerializeField] private Drilling _drilling;
    [SerializeField] private Tile mineralTile;
    [SerializeField] private DynamiteBOOM _boom;
    
    private Tilemap brokenTileMapInstance;


    public int width = 60;
    public int height = 666;

    void Awake()
    {
        Instantiate(backGroundTIleMap, par);
        Instantiate(backMiniMapTilemap, par);
        _drilling._brokenableTilemap = Instantiate(brokenTileMap, par).GetComponent<Tilemap>();
        _drilling._miniMapFrontTilemap = Instantiate(frontMiniMapTilemap, par).GetComponent<Tilemap>();
        _boom._miniMapRockTile = Instantiate(miniMapRockTile, par).GetComponent<Tilemap>();
    }


    void FillGround(Tilemap tilemap, Tile tile)
    {
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y -333, 0), tile);
            }
        }
    }

    
}
