using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Transform enterShopTarget;
    public Transform quitShopTarget;

    public Transform enterInventoryTarget;
    public Transform quitInventoryTarget;

    private float _UIspeed = 0.5f;

    public float delayBeforeShopOpen = 0.5f;

    [SerializeField] private GameObject _shopUI;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _inventoryUIQuitButton;

    [SerializeField] private GameObject _mineralBillButton;
    [SerializeField] private GameObject _mineralSellPanel;
    [SerializeField] private GameObject _mineralNotThingSellPanel;
    [SerializeField] private GameObject _mineralBillImange;


    private Coroutine _shopOpenCoroutine; 
    private bool _isPlayerInsideTrigger = false; 
    public void OnShopQuitButton()
    {

        if (_shopOpenCoroutine != null)
        {
            StopCoroutine(_shopOpenCoroutine);
            _shopOpenCoroutine = null;
        }
        SoundManager.Instance.PlaySFX(SFX.InventoryOpen);
        _mineralSellPanel.SetActive(false);
        _mineralBillButton.SetActive(false);
        _mineralBillImange.SetActive(false);
        _mineralNotThingSellPanel.SetActive(false);
        _shopUI.transform.DOLocalMove(quitShopTarget.localPosition, _UIspeed);
        _inventoryUI.transform.DOLocalMove(quitInventoryTarget.localPosition, _UIspeed);
        //playerMovement.IsMovementLocked = false;
        _isPlayerInsideTrigger = false;
        _inventoryUIQuitButton.SetActive(true);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (_shopOpenCoroutine == null && !_isPlayerInsideTrigger)
            {
                _isPlayerInsideTrigger = true; 
                _shopOpenCoroutine = StartCoroutine(OpenShopWithDelay());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_shopOpenCoroutine != null)
            {
                StopCoroutine(_shopOpenCoroutine);
                _shopOpenCoroutine = null;
            }
            _isPlayerInsideTrigger = false; 
        }
    }



    private IEnumerator OpenShopWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShopOpen);

        if (_isPlayerInsideTrigger)
        {
            _mineralBillButton.SetActive(true);
            _shopUI.transform.DOLocalMove(enterShopTarget.localPosition, _UIspeed);
            _inventoryUI.transform.DOLocalMove(enterInventoryTarget.localPosition, _UIspeed);
            SoundManager.Instance.PlaySFX(SFX.InventoryOpen);
            _inventoryUIQuitButton.SetActive(false);
            //playerMovement.IsMovementLocked = true;
            InventoryUI inventoryUI = _inventoryUI.GetComponent<InventoryUI>();
            inventoryUI.Refresh();
        }
        _shopOpenCoroutine = null;
    }
}




