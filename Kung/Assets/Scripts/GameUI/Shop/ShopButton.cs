using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
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
/// Json���� �ҷ��� �����ͷ� ������ ������ ������ ��ư ������ ��ư Ŭ���� ��ȣ�ۿ��� ����
/// </summary>
public class ShopButton : MonoBehaviour
{
    [Header("������ ������ �θ�")]
    [SerializeField] private RectTransform buttonParent;

    [Header("������ ������")]
    [SerializeField] private GameObject _shopButtonPrefab;

    [Header("��ư Ŭ���� �ؽ�Ʈ ����� ���� ��ҵ�")]
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
                continue; // ���� ī�װ��� ����
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
            throw new IndexOutOfRangeException("�ε��� �ʰ�");

        }
        Item item = _shopItems[index];
        _shopText.GetItemId(item.Id);
        _shopText.SetText(item.DevName, item.Price, item.Discription);
        Debug.Log($"������ �̸� : {item.DevName}, ������ ���� : {item.Price}, ������ ���̵� : {item.Id}");
        
    }
}