using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPopulator : MonoBehaviour
{
    [Header("미리 만든 타일맵 연결")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tile groundTile;

    [SerializeField] private Tilemap brokenTilemap;
    [SerializeField] private Tile brokenTile;

    [SerializeField] private Tilemap frontMiniMapTilemap;
    [SerializeField] private Tile frontMiniMapTile;

    [SerializeField] private Tilemap backMiniMapTilemap;
    [SerializeField] private Tile backMiniMapTile;

    

    [SerializeField] private Tilemap mineralTilemap;
    [SerializeField] private Tile mineralTile;

    public int width = 60;
    public int height = 666;

    void Start()
    {
        FillGround(brokenTilemap, brokenTile);
        FillGround(groundTilemap, groundTile);
        FillGround(frontMiniMapTilemap, frontMiniMapTile);
        FillGround(backMiniMapTilemap, backMiniMapTile);


        //PlaceMinerals();
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

    void PlaceMinerals()
    {
        for (int i = 0; i < 20; i++)
        {
            int x = Random.Range(-width / 2, width / 2);
            int y = Random.Range(-height / 2, height / 2);
            mineralTilemap.SetTile(new Vector3Int(x, y, 0), mineralTile);
        }
    }
}
