using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopItem
{
    public string Id;
    public string ItemName;
    public int Price;      
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


/// <summary>
/// Json에서 불러온 데이터로 상점에 보여줄 아이템 버튼 생성과 버튼 클릭시 상호작용을 구현
/// </summary>
public class ShopButton : MonoBehaviour
{
    [Header("아이템 생성할 부모")]
    [SerializeField] private RectTransform buttonParent;

    [Header("생성할 프리팹")]
    [SerializeField] private GameObject _shopButtonPrefab;

    [Header("버튼 클릭시 텍스트 출력을 위한 요소들")]
    [SerializeField] private ShopText _shopText;
    [SerializeField] private GameObject TextPanel;

    private ShopItem[] _shopItems;
    const string path = "Items/";
    private void Awake()
    {

        // Resources 폴더에서 JSON 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("ShopItems");

        if (jsonFile == null)
        {
            throw new Exception("json없음");
        }

        _shopItems = JsonHelper.FromJson<ShopItem>(jsonFile.text);

        for (int i = 0; i < _shopItems.Length; i++)
        {
            int itemIndex = i;
            GameObject go = Instantiate(_shopButtonPrefab, buttonParent);

            go.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(path + _shopItems[i].Id.ToString());
            
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = _shopItems[i].ItemName;
            texts[1].text = _shopItems[i].Price.ToString();
            
            go.GetComponent<Button>().onClick.AddListener(() => OnClickShopItem(itemIndex));
        }
    }

    private void OnClickShopItem(int index)
    {
        if (_shopItems == null || index >= _shopItems.Length)
        {
            throw new IndexOutOfRangeException("인덱스 초과");

        }
        ShopItem item = _shopItems[index];
        _shopText.SetText(item.ItemName, item.Price, item.Discription);
        Debug.Log($"아이템 이름 : {item.ItemName}, 아이템 가격 : {item.Price}");
        
    }
}