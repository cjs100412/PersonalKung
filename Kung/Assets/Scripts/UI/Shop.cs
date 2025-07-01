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

    public float delayBeforeShopOpen = 0.5f;

    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private PlayerMovement playerMovement;


    private Coroutine _shopOpenCoroutine; // 코루틴 참조를 저장할 변수
    private bool _isPlayerInsideTrigger = false; // 플레이어가 트리거 안에 있는지 확인하는 플래그

    public void OnShopQuitButton()
    {

        // 상점 종료 시 진행 중인 코루틴이 있다면 중지합니다.
        if (_shopOpenCoroutine != null)
        {
            StopCoroutine(_shopOpenCoroutine);
            _shopOpenCoroutine = null;
        }


        ShopUI.transform.DOLocalMove(quitShopTarget.localPosition, _UIspeed);
        InventoryUI.transform.DOLocalMove(quitInventoryTarget.localPosition, _UIspeed);

        playerMovement.IsMovementLocked = false;
        _isPlayerInsideTrigger = false; // 플레이어가 트리거를 벗어났으므로 플래그 초기화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // 이미 코루틴이 실행 중이거나 플레이어가 이미 트리거 안에 있다면 다시 시작하지 않습니다.
            if (_shopOpenCoroutine == null && !_isPlayerInsideTrigger)
            {
                _isPlayerInsideTrigger = true; // 플레이어가 트리거 안에 들어왔음을 표시
                _shopOpenCoroutine = StartCoroutine(OpenShopWithDelay());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 트리거에서 나갔을 때, 상점 열기 코루틴을 중지합니다.
            // 이렇게 하면 플레이어가 진입 라인에서 잠시 머물다 나가도 상점이 열리지 않습니다.
            if (_shopOpenCoroutine != null)
            {
                StopCoroutine(_shopOpenCoroutine);
                _shopOpenCoroutine = null;
            }
            _isPlayerInsideTrigger = false; // 플레이어가 트리거를 벗어났음을 표시
            // 만약 상점 UI가 이미 열려있었다면, 여기에서 닫는 로직을 추가할 수도 있습니다.
            // 하지만 현재는 OnShopQuitButton()을 통해서만 닫히므로, 여기서는 단순히 코루틴 중단만 합니다.
        }
    }



    private IEnumerator OpenShopWithDelay()
    {
        // 지정된 시간만큼 대기합니다.
        yield return new WaitForSeconds(delayBeforeShopOpen);

        // 대기 후에도 플레이어가 여전히 트리거 안에 있다면 상점을 엽니다.
        // 이렇게 다시 확인하는 이유는 플레이어가 지연 시간 동안 트리거 밖으로 나갈 수도 있기 때문입니다.
        if (_isPlayerInsideTrigger)
        {
            ShopUI.transform.DOLocalMove(enterShopTarget.localPosition, _UIspeed);
            InventoryUI.transform.DOLocalMove(enterInventoryTarget.localPosition, _UIspeed);

            playerMovement.IsMovementLocked = true;
        }
        _shopOpenCoroutine = null; // 코루틴이 완료되었으므로 참조를 null로 설정 
    }
}




