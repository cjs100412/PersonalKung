using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamiteBOOM : MonoBehaviour
{
    [SerializeField] private GameObject _Effect;

    private Tilemap _rockTile;
    private RockTile _rockTileScript;

    private void Awake()
    {
        _rockTile = GameObject.Find("RockTile").GetComponent<Tilemap>();
        _rockTileScript = GameObject.Find("RockTile").GetComponent<RockTile>();

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
        _rockTileScript.DestroyRockTile(DynamiteCell);

        SoundManager.Instance.PlaySFX(SFX.Dynamite);
        Destroy(Instantiate(_Effect, transform.position, Quaternion.identity), 3);
    }
}
