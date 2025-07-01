using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPopulator : MonoBehaviour
{
    [SerializeField] Transform par;
    [Header("미리 만든 타일맵 연결")]
    [SerializeField] private GameObject brokenTileMap;
    [SerializeField] private GameObject backGroundTIleMap;
    [SerializeField] private GameObject frontMiniMapTilemap;
    [SerializeField] private GameObject backMiniMapTilemap;
    [SerializeField] private GameObject mineralTilemap;

    [SerializeField] private Drilling _drilling;





   

    void Awake()
    {
        Instantiate(backGroundTIleMap, par);
        Instantiate(brokenTileMap, par);
        Instantiate(frontMiniMapTilemap, par);
        Instantiate(backMiniMapTilemap, par);
        Instantiate(mineralTilemap, par);
        _drilling._brokenableTilemap = brokenTileMap.GetComponent<Tilemap>();  
        
    }

    
    
}
