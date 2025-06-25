using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum InputLockState
{
    None,
    Any,
    Left,
    Right
}
public class PlayerMovement : MonoBehaviour
{
    // 플레이어에 부착할 컴포넌트
    // 플레이어의 이동 제어

    [SerializeField] private float speed;
    private InputLockState currentState = InputLockState.Any;
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator boostAnimator;
    [SerializeField] private Animator drillAnimator;

    [Header("부스트 기능")]
    public float boostPower = 7f; // 부스트 상승파워
    public float maxBoostSpeed = 2f;
    private bool isBoost; // 부스트상태 확인 bool

    [Header("낙하 최대속도 제한")]
    public float maxFallSpeed = -5f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 이 컴포넌트가 활성화 됐을 때, 대리자에 HandleBoostInput 함수를 등록함 (구독)
    private void OnEnable()
    {
        Booster.OnBoostInput += HandleBoostInput;
    }

    // 이 컴포넌트가 비활성화 됐을 때, 대리자의 이벤트 구독을 해제함.
    private void OnDisable()
    {
        Booster.OnBoostInput -= HandleBoostInput;
    }

    private void HandleBoostInput(bool isPressed)
    {
        isBoost = isPressed;
        boostAnimator.SetBool("isBoost", isPressed);
    }

    private void Update()
    {
        // 부스터, 최대속도 제한
        if (isBoost && rb.linearVelocity.y < maxBoostSpeed)
        {
            rb.AddForce(new Vector2(0, boostPower) * Time.deltaTime * 100, ForceMode2D.Force);
        }

        // 낙하 속도 제한
        if(rb.linearVelocityY < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, maxFallSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && currentState != InputLockState.Left)
        {
            currentState = InputLockState.Right;
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rb.linearVelocity = new Vector2(- 1 * speed, rb.linearVelocityY);
           
            ChangeAnimation("MoveLeft", bodyAnimator);
            ChangeAnimation("MoveLeft", headAnimator);
            ChangeAnimation("MoveLeft", drillAnimator);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentState != InputLockState.Right)
        {
            currentState = InputLockState.Left;
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocityY);

            ChangeAnimation("MoveRight", bodyAnimator);
            ChangeAnimation("MoveRight", headAnimator);
            ChangeAnimation("MoveRight", drillAnimator);
        }
        else
        {
            currentState = InputLockState.Any;
            rb.linearVelocity = new Vector2(0 , rb.linearVelocityY);

            ChangeAnimation("Idle",bodyAnimator);
            ChangeAnimation("Idle",headAnimator);
            ChangeAnimation("Idle",drillAnimator);
        }


    }

    // 켜고싶은 불타입 애니메이션 파라미터 이름 , 애니메이터 넣어
    private void ChangeAnimation(string aniName, Animator animator)
    {
        var parameters = animator.parameters;

        foreach (var parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }
        animator.SetBool(aniName, true);

    }
}
