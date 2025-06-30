using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Transform enterShopTarget;
    public Transform quitShopTarget;

    public Transform enterInventoryTarget;
    public Transform quitInventoryTarget;

    private const float DIST = 0.1f;
    private const int TRANSITIONSPEED = 10;

    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject InventoryUI;

    public void OnShopQuitButton()
    {
        StartCoroutine(QuitShopUI());
        StartCoroutine(QuitInventoryUI());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("»óÁ¡ Á¢ÃË");
            StartCoroutine(EnterShopUI());
            StartCoroutine(EnterInventoryUI());
        }
    }
    
    IEnumerator EnterInventoryUI()
    {
        while (Vector3.Distance(InventoryUI.transform.localPosition, enterInventoryTarget.localPosition) > DIST)
        {
            InventoryUI.transform.localPosition = Vector3.Lerp(InventoryUI.transform.localPosition, enterInventoryTarget.localPosition, Time.deltaTime * TRANSITIONSPEED);
            yield return null;
        }
        InventoryUI.transform.localPosition = enterInventoryTarget.localPosition;
    }

    IEnumerator QuitInventoryUI()
    {
        while (Vector3.Distance(InventoryUI.transform.localPosition, quitInventoryTarget.localPosition) > DIST)
        {
            InventoryUI.transform.localPosition = Vector3.Lerp(InventoryUI.transform.localPosition, quitInventoryTarget.localPosition, Time.deltaTime * TRANSITIONSPEED);
            yield return null;
        }
        InventoryUI.transform.localPosition = quitInventoryTarget.localPosition;
    }


    IEnumerator EnterShopUI()
    {
        while (Vector3.Distance(ShopUI.transform.localPosition, enterShopTarget.localPosition) > DIST)
        {
            ShopUI.transform.localPosition = Vector3.Lerp(ShopUI.transform.localPosition, enterShopTarget.localPosition, Time.deltaTime * TRANSITIONSPEED);
            yield return null;
        }
        ShopUI.transform.localPosition = enterShopTarget.localPosition;
    }
    IEnumerator QuitShopUI()
    {
        while (Vector3.Distance(ShopUI.transform.localPosition, quitShopTarget.localPosition) > DIST)
        {
            ShopUI.transform.localPosition = Vector3.Lerp(ShopUI.transform.localPosition, quitShopTarget.localPosition, Time.deltaTime * TRANSITIONSPEED);
            yield return null;
        }
        ShopUI.transform.localPosition = quitShopTarget.localPosition;
    }
}
