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
    [SerializeField] private GameObject InventoryUI;
    //[SerializeField] private PlayerMovement playerMovement;


    private Coroutine _shopOpenCoroutine; 
    private bool _isPlayerInsideTrigger = false; 
    public void OnShopQuitButton()
    {

        if (_shopOpenCoroutine != null)
        {
            StopCoroutine(_shopOpenCoroutine);
            _shopOpenCoroutine = null;
        }


        _shopUI.transform.DOLocalMove(quitShopTarget.localPosition, _UIspeed);
        InventoryUI.transform.DOLocalMove(quitInventoryTarget.localPosition, _UIspeed);

        //playerMovement.IsMovementLocked = false;
        _isPlayerInsideTrigger = false; 
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
            _shopUI.transform.DOLocalMove(enterShopTarget.localPosition, _UIspeed);
            InventoryUI.transform.DOLocalMove(enterInventoryTarget.localPosition, _UIspeed);

            //playerMovement.IsMovementLocked = true;
        }
        _shopOpenCoroutine = null;
    }
}




