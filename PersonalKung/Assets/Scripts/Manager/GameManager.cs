using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Burst.Intrinsics;

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
    public List<string> DeadMonsterList = new List<string>();
}

public class GameManager : MonoBehaviour
{
    private ShortCutServiceLocatorSO _shortCutServiceLocator;

    private string _savePath;
    private PlayerData _data;
    private TileManager _tileManager;
    private MineralTile _mineralTile;
    private RockTile _rockTile;
    private HUD _ui;
    [HideInInspector] public List<string> deadMonsterList;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "KungGameScene")
        {
            _tileManager = FindAnyObjectByType<TileManager>();
            _mineralTile = FindAnyObjectByType<MineralTile>();
            _rockTile = FindAnyObjectByType<RockTile>();
            _ui = FindAnyObjectByType<HUD>();

            LoadGame();
        }
    }

    public void Init(ShortCutServiceLocatorSO shortCutServiceLocator)
    {
        _shortCutServiceLocator = shortCutServiceLocator;
        _tileManager = FindAnyObjectByType<TileManager>();
        _mineralTile = FindAnyObjectByType<MineralTile>();
        _rockTile = FindAnyObjectByType<RockTile>();
        _ui = FindAnyObjectByType<HUD>();
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
            equippedDrillId = 0,
            DeadMonsterList = new List<string>()
        };
        File.WriteAllText(_savePath, JsonUtility.ToJson(_data, true));
        Debug.Log($"[GameManager] Default save created at {_savePath}");
    }

    public void SaveGame(Vector2 pos, int hp, int coins, int score, List<DestroiedTiles> destroiedTiles,
                         List<DestroiedTiles> destroiedMineralTiles,
                         List<DestroiedTiles> destroiedRockTiles,
                         List<UserInventoryItemDto> inv, List<UserShortCutItemDto> sc,
                         int helmId, int bootsId, int drillId, List<string> deadMonsterList)
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
        _data.DeadMonsterList = deadMonsterList;
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
        deadMonsterList = _data.DeadMonsterList;
        Time.timeScale = 1f;
        yield return null;
    }

    public bool IsMonsterCollected(string monsterID)
    {
        return deadMonsterList.Contains(monsterID);
    }


    public void SetMonsterCollected(string monsterID)
    {
        if (!deadMonsterList.Contains(monsterID))
        {
            deadMonsterList.Add(monsterID);
            Debug.Log($"[GameManager] Item collected recorded: {monsterID}");
        }
    }

    // JSON 갱신만 담당하는 내부 함수
    void SaveJSON()
    {
        string json = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_savePath, json);
    }
}