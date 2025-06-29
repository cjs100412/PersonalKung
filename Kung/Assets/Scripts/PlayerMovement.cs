using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum InputLockState
{
    None,
    Any,
    Left,
    Right
}

[RequireComponent(typeof(Rigidbody2D), typeof(Drilling))]
public class PlayerMovement : MonoBehaviour
{
    // === 상태 ===
    private InputLockState currentState = InputLockState.Any;

    private bool _isBoost;
    private bool _isDirectionMoving;
    private bool _isGround;

    // === 타이머 ===
    private float _isGroundTimer;
    private float _boostTimer;

    // === 컴포넌트 ===
    private Rigidbody2D rigidBody;
    private Drilling _drilling;

    [Header("애니메이터")]
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Animator _boostAnimator;
    [SerializeField] private Animator _drillSide;
    [SerializeField] private Animator _drillDown;
    //[SerializeField] private Animator _drillRight;

    [Header("스프라이트")]
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _headSprite;
    [SerializeField] private Sprite[] _drillingSprite;

    [Header("이동 속도")]
    [SerializeField] private float _playerSpeed;

    [Header("부스트 설정")]
    public float boostRisePower = 7f;
    public float maxBoostSpeed = 2f;

    [Header("낙하 제한")]
    public float maxFallSpeed = -5f;

    [SerializeField] private PlayerHealth _playerHealth;
    public CinemachineImpulseSource impulseSource; // 카메라 진동관련
    private Coroutine _drillCoroutine;

    public float shakeInterval = 0.1f;
    private float _shakeTimer;
    // === 생명주기 ===
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _drilling = GetComponent<Drilling>();
    }

    private void OnEnable()
    {
        Booster.OnBoostInput += HandleBoostInput;
        Direction.OnLeftInput += HandleLeftInput;
        Direction.OnLeftRelease += HandleLeftRelease;
        Direction.OnRightInput += HandleRightInput;
        Direction.OnRightRelease += HandleRightRelease;
    }

    private void OnDisable()
    {
        Booster.OnBoostInput -= HandleBoostInput;
        Direction.OnLeftInput -= HandleLeftInput;
        Direction.OnLeftRelease -= HandleLeftRelease;
        Direction.OnRightInput -= HandleRightInput;
        Direction.OnRightRelease -= HandleRightRelease;
    }

    // === 이벤트 핸들러 ===
    private void HandleBoostInput(bool isPressed)
    {
        _boostTimer = 0;
        _isBoost = isPressed;
        _boostAnimator.SetBool("isBoost", isPressed);
    }

    private void HandleLeftInput()
    {
        _isDirectionMoving = true;
        MoveHorizontal(-1,  InputLockState.Right, CurrentDirectionState.Left);
    }

    private void HandleRightInput()
    {
        _isDirectionMoving = true;
        MoveHorizontal(1,  InputLockState.Left, CurrentDirectionState.Right);
    }

    private void HandleLeftRelease() => _isDirectionMoving = false;
    private void HandleRightRelease() => _isDirectionMoving = false;

    // === 이동 로직 ===
    private void Update()
    {
        //bool isDrilling = Input.GetKey(KeyCode.X) && _isGround;

        // 애니메이션 처리만 따로
        if (_drilling.isDrilling)
        {
            _shakeTimer += Time.deltaTime;

            if (_shakeTimer >= shakeInterval)
            {
                impulseSource.GenerateImpulse();
                _shakeTimer = 0f;
            }
        }
        else
        {
            _shakeTimer = 0f; // 멈췄을 때 타이머 리셋
        }
        if (Input.GetKeyDown(KeyCode.X) && _isGround)
        {
            _drilling.isDrilling = true;
            if (_drillCoroutine == null)
                _drillCoroutine = StartCoroutine(_drilling.DrillingRoutine());
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            _drilling.isDrilling = false;

            if (_drillCoroutine != null)
            {
                StopCoroutine(_drillCoroutine);
                _drillCoroutine = null;
            }
        }

        _boostTimer += Time.deltaTime;
        HandleGroundDetection();
        HandleBoostMovement();
        HandleFallSpeedLimit();

        HandleManualInput(); // 키보드 테스트용
    }

    private void HandleManualInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && currentState != InputLockState.Left)
        {
            MoveHorizontal(-1, InputLockState.Right, CurrentDirectionState.Left);
            if (_drilling.isDrilling) StartDrillingLeft(); else StopDrilling();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentState != InputLockState.Right)
        {
            MoveHorizontal(1, InputLockState.Left, CurrentDirectionState.Right);
            if (_drilling.isDrilling) StartDrillingRight(); else StopDrilling();
        }
        else
        {
            IdleState();
        }
    }

    private void MoveHorizontal(float direction, InputLockState lockState, CurrentDirectionState drillDir)
    {
        if (direction == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }


        currentState = lockState;
        _drilling.currentDirectionState = drillDir;
        rigidBody.linearVelocity = new Vector2(direction * _playerSpeed, rigidBody.linearVelocity.y);
        ChangeAnimation("Move", _bodyAnimator);
        ChangeAnimation("Move", _headAnimator);
        _drillDown.SetBool("Stop", true);
        _drillDown.SetBool("Start", false);
        if (_drilling.isDrilling)
        {
            _drillSide.SetBool("IsSide", true);
            _drillSide.SetBool("Stop", false);
            _drillSide.SetBool("Start", true);
        }
        else
        {
            _drillSide.SetBool("Stop", true);
            _drillSide.SetBool("Start", false);
        }

    }

    private void IdleState()
    {
        currentState = InputLockState.Any;
        _drilling.currentDirectionState = CurrentDirectionState.Down;
        rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);

        ChangeAnimation("Down", _bodyAnimator);
        ChangeAnimation("Down", _headAnimator);
        _drillSide.SetBool("IsSide", false);
        _drillSide.SetBool("Stop", true);
        _drillSide.SetBool("Start", false);
        if (_drilling.isDrilling)
        {
            _drillDown.SetBool("Stop", false);
            _drillDown.SetBool("Start", true);
            
            StartDrillingDown();
            StartSmiling();
        }
        else
        {
            _drillDown.SetBool("Stop", true);
            _drillDown.SetBool("Start", false);
            StopDrilling();
            StopSmiling();
        }

        //_drillLeft.SetBool("DrillingLeft", false);
        //_drillRight.SetBool("DrillingRight", false);
    }

    private void HandleGroundDetection()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.red);
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("BrokenableTile"));

        if (hit.collider != null)
        {
            if (_isGroundTimer >= 0.6f && _boostTimer >= 0.8f)
                _playerHealth.TakeDamage(20);

            _isGround = true;
            _isGroundTimer = 0;
        }
        else
        {
            _isGround = false;
            _isGroundTimer += Time.deltaTime;
        }
    }

    private void HandleBoostMovement()
    {
        if ((_isBoost || Input.GetKey(KeyCode.UpArrow)) && rigidBody.linearVelocity.y < maxBoostSpeed)
        {
            rigidBody.AddForce(Vector2.up * boostRisePower * Time.deltaTime * 100, ForceMode2D.Force);
        }
    }

    private void HandleFallSpeedLimit()
    {
        if (rigidBody.linearVelocity.y < maxFallSpeed)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, maxFallSpeed);
        }
    }

    // === 드릴 & 표정 ===
    private void StartDrillingLeft() => SetDrillingSprite(0);
    private void StartDrillingRight() => SetDrillingSprite(1);
    private void StartDrillingDown() => SetDrillingSprite(2);
    private void StartSmiling() => SetHeadSprite(3);

    private void StopDrilling() => _bodyAnimator.enabled = true;
    private void StopSmiling() => _headAnimator.enabled = true;

    private void SetDrillingSprite(int index)
    {
        _bodyAnimator.enabled = false;
        _bodySprite.sprite = _drillingSprite[index];
    }

    private void SetHeadSprite(int index)
    {
        _headAnimator.enabled = false;
        _headSprite.sprite = _drillingSprite[index];
    }

    // === 애니메이션 전환 ===
    private void ChangeAnimation(string aniName, Animator animator)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(param.name, false);
        }
        animator.SetBool(aniName, true);
    }
}
