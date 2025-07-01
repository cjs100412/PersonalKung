using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

// Unity 인스펙터에서 이 구조체를 편집할 수 있도록 [Serializable] 어트리뷰트 추가
[Serializable]
public class ShopItem
{
    public string Id;
    public string ItemName; // 아이템 이름 (예: "구급상자")
    public int Price;       // 아이템 가격 (예: 1000)
    public string Discription;
}
[Serializable]
public class ShopItemList
{
    public ShopItem[] items;
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrappedJson = "{\"array\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}



public class ShopButton : MonoBehaviour
{
    [SerializeField] private Button[] _shopButtons;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private ShopText _shop;
    private ShopItem[] _shopItems;
    private void Awake()
    {

        // Resources 폴더에서 JSON 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("ShopItems");
        if (jsonFile == null)
        {
            Debug.LogError("ShopItems_RootArray.json 파일이 Resources 폴더에 없습니다.");
            return;
        }

        _shopItems = JsonHelper.FromJson<ShopItem>(jsonFile.text);

        for (int i = 0; i < _shopButtons.Length; i++)
        {
            int itemIndex = i;
            _shopButtons[i].onClick.AddListener(() => OnClickShopItem(itemIndex));
        }
    }

    private void OnClickShopItem(int index)
    {
        if (_shopItems == null || index >= _shopItems.Length)
        {
            Debug.LogWarning("아이템 정보가 없습니다.");
            return;
        }
        ShopItem item = _shopItems[index];
        _shop.SetText(item.ItemName, item.Price, item.Discription);
        Debug.Log($"아이템 이름 : {item.ItemName}, 아이템 가격 : {item.Price}");
        
    }
}