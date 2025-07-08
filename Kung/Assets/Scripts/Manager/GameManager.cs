using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Overlays;

[System.Serializable]
public class PlayerData
{
    public int hp;
    public int coins;
    public int score;
    public int equippedHelmetId;
    public int equippedBootsId;
    public int equippedDrillId;
    public List<UserInventoryItemDto> inventoryItems = new List<UserInventoryItemDto>();
    public List<UserShortCutItemDto> shortCutItems = new List<UserShortCutItemDto>();
    public List<DestroiedTiles> destroiedTiles = new List<DestroiedTiles>();
    public List<DestroiedTiles> destroiedMineralTiles = new List<DestroiedTiles>();
    public List<DestroiedTiles> destroiedRockTiles = new List<DestroiedTiles>();
}

public class GameManager : MonoBehaviour
{
    private ShortCutServiceLocatorSO _shortCutServiceLocator;

    private string _savePath;
    private PlayerData _data;
    private TileManager _tileManager;
    private MineralTile _mineralTile;
    private RockTile _rockTile;
    private UI _ui;

    public void Init(ShortCutServiceLocatorSO shortCutServiceLocator)
    {
        _shortCutServiceLocator = shortCutServiceLocator;
        _tileManager = FindAnyObjectByType<TileManager>();
        _mineralTile = FindAnyObjectByType<MineralTile>();
        _rockTile = FindAnyObjectByType<RockTile>();
        _ui = FindAnyObjectByType<UI>();
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(_savePath))
        {
            var json = File.ReadAllText(_savePath);
            _data = JsonUtility.FromJson<PlayerData>(json);
            if (_data.inventoryItems == null) _data.inventoryItems = new();
            if (_data.shortCutItems == null) _data.shortCutItems = new();
        }
        else
        {
            InitializeDefaultSave();
        }
    }

    private void Start()
    {
        LoadGame();
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeDefaultSave()
    {
        _data = new PlayerData
        {
            hp = 100,
            coins = 0,
            score = 0,
            destroiedTiles = new List<DestroiedTiles>(),
            destroiedMineralTiles = new List<DestroiedTiles>(),
            destroiedRockTiles = new List<DestroiedTiles>(),
            inventoryItems = new(),
            shortCutItems = _shortCutServiceLocator.Service.Items,
            equippedHelmetId = 0,
            equippedBootsId = 0,
            equippedDrillId = 0
        };
        File.WriteAllText(_savePath, JsonUtility.ToJson(_data, true));
        Debug.Log($"[GameManager] Default save created at {_savePath}");
    }

    public void SaveGame(Vector2 pos, int hp, int coins, int score, List<DestroiedTiles> destroiedTiles,
                         List<DestroiedTiles> destroiedMineralTiles,
                         List<DestroiedTiles> destroiedRockTiles,
                         List<UserInventoryItemDto> inv, List<UserShortCutItemDto> sc,
                         int helmId, int bootsId, int drillId)
    {
        _data.hp = hp;
        _data.coins = coins;
        _data.score = score;
        _data.destroiedTiles = destroiedTiles;
        _data.destroiedMineralTiles = destroiedMineralTiles;
        _data.destroiedRockTiles = destroiedRockTiles;
        _data.inventoryItems = inv;
        _data.shortCutItems = sc;
        _data.equippedHelmetId = helmId;
        _data.equippedBootsId = bootsId;
        _data.equippedDrillId = drillId;
        File.WriteAllText(_savePath, JsonUtility.ToJson(_data, true));
        Debug.Log("[GameManager] Saved");
    }

    public void LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("[GameManager] No save file found.");
            return;
        }

        var json = File.ReadAllText(_savePath);
        _data = JsonUtility.FromJson<PlayerData>(json);
        StartCoroutine(LoadAndRestore());
    }

    private IEnumerator LoadAndRestore()
    {
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) yield break;

        var ph = playerGO.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.Respawn(
                _data.hp, _data.coins,
                _data.inventoryItems, _data.shortCutItems,
                _data.equippedHelmetId, _data.equippedBootsId, _data.equippedDrillId
            );
        }
        _ui.score = _data.score;
        _tileManager.LoadDestroiedTiles(_data.destroiedTiles);
        _mineralTile.LoadDestroiedTiles(_data.destroiedMineralTiles);
        _rockTile.LoadDestroiedTiles(_data.destroiedRockTiles);
        yield return null;
    }

    public void SetCoins(int newCoinCount)
    {
        _data.coins = newCoinCount;
        SaveJSON();
        Debug.Log($"[GameManager] Coins updated to: {newCoinCount}");
    }


    // JSON 갱신만 담당하는 내부 함수
    void SaveJSON()
    {
        string json = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_savePath, json);
    }
}