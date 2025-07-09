using TMPro;
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
    [Header("채굴 타일맵")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private Tilemap _rockTileMap;
    private Tilemap _brokenableTilemap;
    
    [Header("미니맵 관련")]
    [HideInInspector] private Tilemap _miniMapFrontTilemap;   
    [SerializeField] private TextMeshProUGUI _depthText; 
    
    [Header("플레이어 연결")]
    [SerializeField] private PlayerStats playerStats;


    [Header("부셔지는 타일맵 스프라이트 배열")]
    public Sprite[] brokenTileSprites;

    private int _surfaceY; 

    public LayerMask brokenTileLayer;
    public LayerMask RockTileLayer;
    public float lastDrillTime;
    public float drillCoolTime;
    private int currentDrillDirection = int.MinValue;
    public bool isDrilling = false;

    private void Start()
    {
        _brokenableTilemap = tileManager.brokenableTilemap;
        _miniMapFrontTilemap = tileManager.miniMapFrontTilemap;
    }

    private void Update()
    {
        Vector3Int currentCell = _brokenableTilemap.WorldToCell(transform.position); 
        int depth = Mathf.Max(0, _surfaceY - currentCell.y);   
        _depthText.text = depth + "m";
    }
    public void StartDrilling(int dir)
    {
        if (dir != currentDrillDirection)
        {
            currentDrillDirection = dir;
            lastDrillTime = Time.time; 
        }
    }

    public void StopDrilling()
    {
        currentDrillDirection = int.MinValue; 
    }

    public void ProcessDrilling()
    {
        if (currentDrillDirection != int.MinValue)
        {
            if (Time.time - lastDrillTime >= drillCoolTime)
            {
                // 드릴 동작 수행
                ExecuteDrillAction(currentDrillDirection);
                lastDrillTime = Time.time; 
            }
        }
    }

    public bool CanDrill(int dir)
    {
        Vector2 origin = transform.position;
        Vector2 direction = dir == 0 ? Vector2.down : (dir == 1 ? Vector2.right : Vector2.left);
        bool brokenTileHit = Physics2D.Raycast(origin, direction, 0.2f, brokenTileLayer);
        bool rockTileHit = Physics2D.Raycast(origin, direction, 0.2f, RockTileLayer);
        return brokenTileHit || rockTileHit;
    }


    public void ExecuteDrillAction(int dir)
    {
        Vector3Int cell = _brokenableTilemap.WorldToCell(transform.position);
        Vector3Int rockcell = _rockTileMap.WorldToCell(transform.position);
        Vector3Int target = dir switch
        {
            -1 => new Vector3Int(cell.x - 1, cell.y), // 좌
            1 => new Vector3Int(cell.x + 1, cell.y),  // 우
            0 => new Vector3Int(cell.x, cell.y - 1),  // 아래
            _ => cell 
        };
        Vector3Int rockTarget = dir switch
        {
            -1 => new Vector3Int(rockcell.x - 1, rockcell.y), // 좌
            1 => new Vector3Int(rockcell.x + 1, rockcell.y),  // 우
            0 => new Vector3Int(rockcell.x, rockcell.y - 1),  // 아래
            _ => rockcell
        };

        if (!_brokenableTilemap.HasTile(target))
        {
            StopDrilling(); 
            Debug.Log($"ExecuteDrillAction: No tile at target {target}. Stopping drill.");

            return; 
        }

        tileManager.DamageTile(target, playerStats.drillDamage);

    }
}
