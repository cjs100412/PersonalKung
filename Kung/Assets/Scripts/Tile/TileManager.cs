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

    [SerializeField] private Drilling _drilling;


    private Tilemap brokenTileMapInstance;


    [SerializeField] private Tile mineralTile;

    public int width = 60;
    public int height = 666;

    void Awake()
    {
        Instantiate(backGroundTIleMap, par);
        Instantiate(backMiniMapTilemap, par);
        _drilling._brokenableTilemap = Instantiate(brokenTileMap, par).GetComponent<Tilemap>();
        _drilling._miniMapFrontTilemap = Instantiate(frontMiniMapTilemap, par).GetComponent<Tilemap>();
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
