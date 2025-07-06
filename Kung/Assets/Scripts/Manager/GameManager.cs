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

    // �� óġ ���θ� ������ ����Ʈ
    public List<string> defeatedEnemies = new List<string>();
    // �� �ı� ���θ� ������ ����Ʈ
    public List<string> destroyedWalls = new List<string>();

    // ���� ������ ���� ����
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

        //�����Ϳ��� �׽�Ʈ������ ����
//#if UNITY_EDITOR
//        if (File.Exists(savePath))
//            File.Delete(savePath);
//#endif

        if (File.Exists(savePath))
        {
            // ���� ������ ������ �������� �ʰ� �ε常 �Ѵ�
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<PlayerData>(json);

            // ������ �� ����Ʈ�� null�� ������ ��� ���
            if (data.defeatedEnemies == null) data.defeatedEnemies = new List<string>();
            if (data.destroyedWalls == null) data.destroyedWalls = new List<string>();
            if (data.inventoryItems == null) data.inventoryItems = new List<UserInventoryItemDto>();
            //if (data.shortCutItems == null) data.shortCutItems = new List<UserShortCutItemDto>();

        }
        else
        {
            // ���̺� ������ ���� ���� �ʱⰪ ����
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
        data.hp = 100;         // �⺻ HP
        data.coins = 0;           // �⺻ ����
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
        Debug.Log("Game Saved �� " + savePath + ")");
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
        // ���� �񵿱�� �ε��ϰ�, �ε� �Ϸ� �Ŀ� ��ġ������ ����
        StartCoroutine(LoadAndRestore());
    }

    IEnumerator LoadAndRestore()
    {

        // �÷��̾� ������Ʈ ã��
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogError("Player not found after load!");
            yield break;
        }

        Debug.Log($"LoadAndRestore hp : {data.hp}");
        // ���� ����
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
            // fallback: ���� ����
            var rb = playerGO.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
        }

        yield return null;
    }

    // ������ �̹� óġ�Ǿ������� Ȯ��
    public bool IsBossDefeated(string bossID)
    {
        return data.defeatedEnemies.Contains(bossID);
    }

    // ������ óġ������ ����ϰ� ������� ����
    public void SetBossDefeated(string bossID)
    {
        if (!data.defeatedEnemies.Contains(bossID))
        {
            data.defeatedEnemies.Add(bossID);
            // ��ȭ�� ���¸� ���Ͽ� �ٷ� ����
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"[GameManager] Boss defeated recorded: {bossID}");
        }
    }

    // ���� �̹� �ı��Ǿ������� Ȯ��
    public bool IsWallDestroyed(string wallID)
    {
        return data.destroyedWalls.Contains(wallID);
    }

    // �� �ı��� ����ϰ� ������� ����
    public void SetWallDestroyed(string wallID)
    {
        if (!data.destroyedWalls.Contains(wallID))
        {
            data.destroyedWalls.Add(wallID);
            // ��ȭ�� ���¸� ���Ͽ� �ٷ� ����
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"[GameManager] Wall destroyed recorded: {wallID}");
        }
    }

    // ���� ������ ����
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


    // JSON ���Ÿ� ����ϴ� ���� �Լ�
    void SaveJSON()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
