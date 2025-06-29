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
    private GroundChecker _groundChecker;

    [Header("애니메이터")]
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Animator _boostAnimator;
    [SerializeField] private Animator _drillSide;
    [SerializeField] private Animator _drillDown;

    [Header("스프라이트")]
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _headSprite;
    [SerializeField] private Sprite _drillingHeadSprite;
    [SerializeField] private Sprite _drillingBodySprite;

    [Header("이동 속도")]
    [SerializeField] private float _playerSpeed;

    [Header("부스트 설정")]
    public float boostRisePower = 7f;
    public float maxBoostSpeed = 2f;

    [Header("낙하 제한")]
    public float maxFallSpeed = -5f;


    [Header("카메라 흔들림")]
    public CinemachineImpulseSource impulseSource; // 카메라 진동관련
    public float shakeInterval = 0.1f;
    private float _shakeTimer;

    [Header("스크립트")]
    [SerializeField] private PlayerHealth _playerHealth;

    private Coroutine _drillCoroutine;

    
    // === 생명주기 ===
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _drilling = GetComponent<Drilling>();
        _groundChecker = GetComponent<GroundChecker>();
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
            OnDrillKeyDown();
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            OnDrillKeyUp();
        }

        _boostTimer += Time.deltaTime;
        _isGround = _groundChecker.IsGrounded;
        //HandleGroundDetection();
        HandleBoostMovement();
        HandleFallSpeedLimit();

        HandleManualInput(); // 키보드 테스트용
    }


    private void OnDrillKeyUp()
    {
        _drilling.isDrilling = false;

        if (_drillCoroutine != null)
        {
            StopCoroutine(_drillCoroutine);
            _drillCoroutine = null;
        }
    }


    private void OnDrillKeyDown()
    {
        _drilling.isDrilling = true;
        if (_drillCoroutine == null)
            _drillCoroutine = StartCoroutine(_drilling.DrillingRoutine());
    }


    private void HandleManualInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && currentState != InputLockState.Left)
        {
            
            MoveHorizontal(-1, InputLockState.Right, CurrentDirectionState.Left);
            if (_drilling.isDrilling) StartDrillingSide(); else StopDrilling();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentState != InputLockState.Right)
        {
            
            MoveHorizontal(1, InputLockState.Left, CurrentDirectionState.Right);
            if (_drilling.isDrilling) StartDrillingRight(); else StopDrilling();
        }
        else
        {
            if (_drilling.currentDirectionState == CurrentDirectionState.Left || _drilling.currentDirectionState == CurrentDirectionState.Right) 
            {
                OnDrillKeyUp();
            }
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
        StopDownDirllAni();
        StopSmiling();

        if (_drilling.isDrilling)
        {
            StartDrillingSide();
            StartSidetDirllAni();
        }
        else
        {
            StopDrilling();
            StopSideDirllAni();
        }

    }


    private void IdleState()
    {
        currentState = InputLockState.Any;
        _drilling.currentDirectionState = CurrentDirectionState.Down;
        rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);

        ChangeAnimation("Down", _bodyAnimator);
        ChangeAnimation("Down", _headAnimator);
        StopSideDirllAni();
        StopDrilling();

        if (_drilling.isDrilling)
        {
            StarDowntDirllAni();
            StartSmiling();
        }
        else
        {
            StopDownDirllAni();
            StopSmiling();
        }

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
            OnDrillKeyUp();
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
    private void StartDrillingSide() => SetDrillingSprite();
    private void StartDrillingRight() => SetDrillingSprite();
    private void StartSmiling() => SetHeadSprite();
    private void StopDrilling() => _bodyAnimator.enabled = true;
    private void StopSmiling() => _headAnimator.enabled = true;


    private void SetDrillingSprite()
    {
        _bodyAnimator.enabled = false;
        _bodySprite.sprite = _drillingBodySprite;
    }


    private void SetHeadSprite()
    {
        _headAnimator.enabled = false;
        _headSprite.sprite = _drillingHeadSprite;
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


    private void StarDowntDirllAni()
    {
        _drillDown.SetBool("Stop", false);
        _drillDown.SetBool("Start", true);
    }


    private void StopDownDirllAni()
    {
        _drillDown.SetBool("Stop", true);
        _drillDown.SetBool("Start", false);
    }


    private void StartSidetDirllAni()
    {
        _drillSide.SetBool("IsSide", true);
        _drillSide.SetBool("Stop", false);
        _drillSide.SetBool("Start", true);
    }


    private void StopSideDirllAni()
    {
        _drillSide.SetBool("IsSide", false);
        _drillSide.SetBool("Stop", true);
        _drillSide.SetBool("Start", false);
    }


}
