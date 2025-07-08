using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//public static class JsonHelper
//{
//    public static T[] FromJson<T>(string json)
//    {
//        string wrappedJson = "{\"array\":" + json + "}";
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
//        return wrapper.array;
//    }

//    [Serializable]
//    private class Wrapper<T>
//    {
//        public T[] array;
//    }
//}


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

    [SerializeField] private InventoryRepositoryLocatorSO _itemRepositoryLocator;
    private List<Item> _shopItems;

    const string path = "Textures/";
    private void Start()
    {
        _shopItems = _itemRepositoryLocator.Repository.FindAll() as List<Item>;

        for (int i = 0; i < _shopItems.Count; i++)
        {
            int itemIndex = i;
            if (_shopItems[i].Category == "mineral")
            {
                continue; // 광물 카테고리는 제외
            }
            GameObject go = Instantiate(_shopButtonPrefab, buttonParent);

            go.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(path + _shopItems[i].Id.ToString());
            
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = _shopItems[i].DevName;
            texts[1].text = _shopItems[i].Price.ToString();
            
            go.GetComponent<Button>().onClick.AddListener(() => OnClickShopItem(itemIndex));
        }
    }

    private void OnClickShopItem(int index)
    {
        if (_shopItems == null || index >= _shopItems.Count)
        {
            throw new IndexOutOfRangeException("인덱스 초과");

        }
        Item item = _shopItems[index];
        _shopText.GetItemId(item.Id);
        _shopText.SetText(item.DevName, item.Price, item.Discription);
        Debug.Log($"아이템 이름 : {item.DevName}, 아이템 가격 : {item.Price}, 아이템 아이디 : {item.Id}");
        
    }
}