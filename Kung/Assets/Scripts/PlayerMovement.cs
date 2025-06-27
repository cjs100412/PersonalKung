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
    private InputLockState currentState = InputLockState.Any;

    [SerializeField] private float speed;
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator boostAnimator;

    [SerializeField] private Animator _drillLeft;
    [SerializeField] private Animator _drillRight;

    [Header("부스트 기능")]
    public float boostPower = 7f; // 부스트 상승파워
    public float maxBoostSpeed = 2f;
    private bool isBoost; // 부스트상태 확인 bool
    public bool isDrilling;

    [Header("낙하 최대속도 제한")]
    public float maxFallSpeed = -5f;

    private Rigidbody2D rigidBody;
    private Drilling drilling;
    [SerializeField] private PlayerHealth health;

    private float _isGroundTimer;
    private float _BoostTimer;

    private bool _isGround;
    private bool isDirectionMoving;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private Sprite[] drillSprite;

    void StartDrillingLeft()
    {
        bodyAnimator.enabled = false;
        spriteRenderer.sprite = drillSprite[0];
    }
    void StartDrillingRight()
    {
        bodyAnimator.enabled = false;
        spriteRenderer.sprite = drillSprite[1];
    }
    void StartDrillingDown()
    {
        bodyAnimator.enabled = false;
        spriteRenderer.sprite = drillSprite[2];
    }
    void StopDrilling()
    {
        bodyAnimator.enabled = true;
    }

    void StartSmiling()
    {
        headAnimator.enabled = false;
        headSprite.sprite = drillSprite[3];
    }
    void StopSmiling()
    {
        headAnimator.enabled = true;
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        drilling = GetComponent<Drilling>();
    }

    // 이 컴포넌트가 활성화 됐을 때, 대리자에 HandleBoostInput 함수를 등록함 (구독)
    private void OnEnable()
    {
        Booster.OnBoostInput += HandleBoostInput;
        Direction.OnLeftInput += HandleLeftInput;
        Direction.OnLeftRelease += HandleLeftRelease;
        Direction.OnRightInput += HandleRightInput;
        Direction.OnRightRelease += HandleRightRelease;
    }

    // 이 컴포넌트가 비활성화 됐을 때, 대리자의 이벤트 구독을 해제함.
    private void OnDisable()
    {
        Booster.OnBoostInput -= HandleBoostInput;
        Direction.OnLeftInput -= HandleLeftInput;
        Direction.OnLeftRelease -= HandleLeftRelease;
        Direction.OnRightInput -= HandleRightInput;
        Direction.OnRightRelease -= HandleRightRelease;
    }

    private void HandleBoostInput(bool isPressed)
    {
        _BoostTimer = 0;
        isBoost = isPressed;
        boostAnimator.SetBool("isBoost", isPressed);
    }

    private void HandleLeftInput()
    {
        isDirectionMoving = true;
        currentState = InputLockState.Right;
        drilling.currentDirectionState = CurrentDirectionState.Left;
        rigidBody.linearVelocity = new Vector2(-1 * speed, rigidBody.linearVelocityY);

        ChangeAnimation("MoveLeft", bodyAnimator);
        ChangeAnimation("MoveLeft", headAnimator);
    }

    private void HandleRightInput()
    {
        isDirectionMoving = true;
        currentState = InputLockState.Left;
        drilling.currentDirectionState = CurrentDirectionState.Right;
        rigidBody.linearVelocity = new Vector2(1 * speed, rigidBody.linearVelocityY);

        ChangeAnimation("MoveRight", bodyAnimator);
        ChangeAnimation("MoveRight", headAnimator);
    }

    private void HandleLeftRelease()
    {
        isDirectionMoving = false;
    }

    private void HandleRightRelease()
    {
        isDirectionMoving = false;
    }

    private void Update()
    {
        _BoostTimer += Time.deltaTime;
        Debug.DrawRay(transform.position, new Vector2(0, -0.1f), new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("BrokenableTile"));
        if(rayHit.collider != null)
        {
            if (_isGroundTimer >= 0.6f && _BoostTimer >= 0.8f)
            {
                health.TakeDamage(20);
            }
            _isGround = true;
            _isGroundTimer = 0;
            
        }
        else
        {
            _isGround = false;
            _isGroundTimer += Time.deltaTime;
        }

        // 부스터, 최대속도 제한
        if ((isBoost && rigidBody.linearVelocity.y < maxBoostSpeed) || (Input.GetKey(KeyCode.UpArrow) && rigidBody.linearVelocity.y < maxBoostSpeed))
        {
            rigidBody.AddForce(new Vector2(0, boostPower) * Time.deltaTime * 100, ForceMode2D.Force);
        }
        

        // 낙하 속도 제한
        if (rigidBody.linearVelocityY < maxFallSpeed)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocityX, maxFallSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && currentState != InputLockState.Left)
        {
            currentState = InputLockState.Right;
            drilling.currentDirectionState = CurrentDirectionState.Left;
            rigidBody.linearVelocity = new Vector2(-1 * speed, rigidBody.linearVelocityY);

            ChangeAnimation("MoveLeft", bodyAnimator);
            ChangeAnimation("MoveLeft", headAnimator);
            //_drillLeft.SetBool("DrillingLeft", false);
            if (isDrilling)
            {
                StartDrillingLeft();
            }
            else
            {
                StopDrilling();
            }

        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentState != InputLockState.Right)
        {
            currentState = InputLockState.Left;
            drilling.currentDirectionState = CurrentDirectionState.Right;
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rigidBody.linearVelocity = new Vector2(1 * speed, rigidBody.linearVelocityY);

            if (isDrilling)
            {
                StartDrillingRight();
            }
            else
            {
                StopDrilling();
            }


                ChangeAnimation("MoveRight", bodyAnimator);
            ChangeAnimation("MoveRight", headAnimator);
            //_drillRight.SetBool("DrillingRight", false);
        }
        else
        {
            //if (!isDirectionMoving)
            //{
            //    
            //    //_drilldown.SetBool("DrillingDown", false);
            //    //ChangeAnimation("Idle", drillAnimator);
            //}
            currentState = InputLockState.Any;
            drilling.currentDirectionState = CurrentDirectionState.Down;
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocityY);
            ChangeAnimation("Idle", bodyAnimator);
            ChangeAnimation("Idle", headAnimator);
            if (isDrilling)
            {
                StartDrillingDown();
                StartSmiling();
            }
            else
            {
                StopDrilling();
                StopSmiling();
            }
            _drillLeft.SetBool("DrillingLeft",false);
            _drillRight.SetBool("DrillingRight", false);
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