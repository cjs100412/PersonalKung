using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class DynamiteBOOM : MonoBehaviour
{
    [SerializeField] private GameObject _Effect;
    public Tilemap _miniMapRockTile;

    private Tilemap _rockTile;
    private void Awake()
    {
        _rockTile = GameObject.Find("RockTile").GetComponent<Tilemap>();
        if (_rockTile == null)
        {
            Debug.LogError("RockTilemap not found in the scene.");
        }
    }
    void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnDestroy()
    {
        Vector3Int DynamiteCell = _rockTile.WorldToCell(transform.position);
        Debug.Log($"Explosion at cell {DynamiteCell}, hasTile? {_rockTile.HasTile(DynamiteCell)}");
        if (_rockTile.HasTile(DynamiteCell))
            _rockTile.SetTile(DynamiteCell, null);

        if (_miniMapRockTile != null && _miniMapRockTile.HasTile(DynamiteCell))
            _miniMapRockTile.SetTile(DynamiteCell, null);

        Destroy(Instantiate(_Effect, transform.position, Quaternion.identity), 3);
    }
}
