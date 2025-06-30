using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Transform enterShopTarget;
    public Transform quitShopTarget;

    public Transform enterInventoryTarget;
    public Transform quitInventoryTarget;

    private float _UIspeed = 0.5f;

    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject InventoryUI;

    public void OnShopQuitButton()
    {
        ShopUI.transform.DOLocalMove(quitShopTarget.localPosition, _UIspeed);
        InventoryUI.transform.DOLocalMove(quitInventoryTarget.localPosition, _UIspeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ShopUI.transform.DOLocalMove(enterShopTarget.localPosition, _UIspeed);
            InventoryUI.transform.DOLocalMove(enterInventoryTarget.localPosition, _UIspeed);
        }
    }
}
