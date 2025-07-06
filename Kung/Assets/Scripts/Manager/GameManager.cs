using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int hp;
    public int coins;
    public int equippedHelmetId;
    public int equippedBootsId;
    public int equippedDrillId;
    public List<UserInventoryItemDto> inventoryItems = new List<UserInventoryItemDto>();
    public List<UserShortCutItemDto> shortCutItems = new List<UserShortCutItemDto>();

    // 적 처치 여부를 저장할 리스트
    public List<string> defeatedEnemies = new List<string>();
    // 벽 파괴 여부를 저장할 리스트
    public List<string> destroyedWalls = new List<string>();

    // 쇼핑 아이템 구매 여부
    public bool boughtExpandBag1 = false;
    public bool boughtExpandBag2 = false;
}

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }
    string savePath;
    PlayerData data;
    public int SavedHp => data.hp;
    public int SavedCoins => data.coins;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        data = new PlayerData();

        //에디터에선 테스트용으로 삭제
//#if UNITY_EDITOR
//        if (File.Exists(savePath))
//            File.Delete(savePath);
//#endif

        if (File.Exists(savePath))
        {
            // 기존 파일이 있으면 삭제하지 않고 로드만 한다
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<PlayerData>(json);

            // 데이터 중 리스트가 null로 내려올 경우 대비
            if (data.defeatedEnemies == null) data.defeatedEnemies = new List<string>();
            if (data.destroyedWalls == null) data.destroyedWalls = new List<string>();
            if (data.inventoryItems == null) data.inventoryItems = new List<UserInventoryItemDto>();
            //if (data.shortCutItems == null) data.shortCutItems = new List<UserShortCutItemDto>();

        }
        else
        {
            // 세이브 파일이 없을 때만 초기값 세팅
            InitializeDefaultSave();
        }
        //LoadGame();
    }
    private void Start()
    {
        LoadGame();

    }
    void InitializeDefaultSave()
    {
        data.hp = 100;         // 기본 HP
        data.coins = 0;           // 기본 동전
        data.defeatedEnemies = new List<string>();
        data.destroyedWalls = new List<string>();
        data.inventoryItems = new List<UserInventoryItemDto>();
        //data.shortCutItems = new List<UserShortCutItemDto>();
        data.shortCutItems = _shortCutServiceLocator.Service.Items;
        data.equippedHelmetId = 0;
        data.equippedBootsId = 0;
        data.equippedDrillId = 0;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"[GameManager] Save initialized: {savePath}");
    }

    public void SaveGame(Vector2 pos, int hp,int coins, List<UserInventoryItemDto> inventoryItems, List<UserShortCutItemDto> shortCutItems,
                         int equippedHelmetId, int equippedBootsId, int equippedDrillId)
    {
        data.hp = hp;
        data.coins = coins;
        data.inventoryItems = inventoryItems;
        data.shortCutItems = shortCutItems;
        data.equippedHelmetId = equippedHelmetId;
        data.equippedBootsId = equippedBootsId;
        data.equippedDrillId = equippedDrillId;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved → " + savePath + ")");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file.");
            return;
        }

        string json = File.ReadAllText(savePath);
        data = JsonUtility.FromJson<PlayerData>(json);

        if (data.defeatedEnemies == null) data.defeatedEnemies = new List<string>();
        if (data.destroyedWalls == null) data.destroyedWalls = new List<string>();
        if (data.inventoryItems == null) data.inventoryItems = new List<UserInventoryItemDto>();
        if (data.shortCutItems == null) data.shortCutItems = new List<UserShortCutItemDto>();
        // 씬을 비동기로 로드하고, 로드 완료 후에 위치·상태 복원
        StartCoroutine(LoadAndRestore());
    }

    IEnumerator LoadAndRestore()
    {

        // 플레이어 오브젝트 찾기
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogError("Player not found after load!");
            yield break;
        }

        Debug.Log($"LoadAndRestore hp : {data.hp}");
        // 상태 복원
        int hp = data.hp;
        int coins = data.coins;
        List<UserInventoryItemDto> inventoryItems = data.inventoryItems;
        List<UserShortCutItemDto> shortCutItems = data.shortCutItems;
        int equippedHelmetId = data.equippedHelmetId;
        int equippedBootsId = data.equippedBootsId;
        int equippedDrillId = data.equippedDrillId;

        Debug.Log("Player state restored: HP=" + data.hp + " Coins=" + data.coins);

        var ph = playerGO.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.Respawn(hp, coins, inventoryItems, shortCutItems, equippedHelmetId, equippedBootsId, equippedDrillId);
        }
        else
        {
            // fallback: 직접 복원
            var rb = playerGO.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
        }

        yield return null;
    }

    // 보스가 이미 처치되었는지를 확인
    public bool IsBossDefeated(string bossID)
    {
        return data.defeatedEnemies.Contains(bossID);
    }

    // 보스를 처치했음을 기록하고 저장까지 수행
    public void SetBossDefeated(string bossID)
    {
        if (!data.defeatedEnemies.Contains(bossID))
        {
            data.defeatedEnemies.Add(bossID);
            // 변화된 상태를 파일에 바로 저장
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"[GameManager] Boss defeated recorded: {bossID}");
        }
    }

    // 벽이 이미 파괴되었는지를 확인
    public bool IsWallDestroyed(string wallID)
    {
        return data.destroyedWalls.Contains(wallID);
    }

    // 벽 파괴를 기록하고 저장까지 수행
    public void SetWallDestroyed(string wallID)
    {
        if (!data.destroyedWalls.Contains(wallID))
        {
            data.destroyedWalls.Add(wallID);
            // 변화된 상태를 파일에 바로 저장
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"[GameManager] Wall destroyed recorded: {wallID}");
        }
    }

    // 상점 아이템 상태
    public bool IsBoughtExpandBag1() { return data.boughtExpandBag1; }
    public bool IsBoughtExpandBag2() { return data.boughtExpandBag2; }

    public void SetBoughtExpandBag1()
    {
        if (!data.boughtExpandBag1)
        {
            data.boughtExpandBag1 = true;
            SaveJSON();
            Debug.Log("[GameManager] Bought ExpandBag1");
        }
    }


    public void SetBoughtExpandBag2()
    {
        if (!data.boughtExpandBag2)
        {
            data.boughtExpandBag2 = true;
            SaveJSON();
            Debug.Log("[GameManager] Bought ExpandBag2");
        }
    }


    public void SetCoins(int newCoinCount)
    {
        data.coins = newCoinCount;
        SaveJSON();
        Debug.Log($"[GameManager] Coins updated to: {newCoinCount}");
    }


    // JSON 갱신만 담당하는 내부 함수
    void SaveJSON()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
