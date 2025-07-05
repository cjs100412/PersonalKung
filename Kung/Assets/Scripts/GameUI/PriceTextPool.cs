using System.Collections.Generic;
using UnityEngine;

public class PriceTextPool : MonoBehaviour
{
    [SerializeField] private GameObject priceTextPrefab;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(priceTextPrefab, canvasTransform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go;
        }
        else
        {
            // 풀 부족 시 추가 생성 (선택사항)
            GameObject go = Instantiate(priceTextPrefab, canvasTransform);
            return go;
        }
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
