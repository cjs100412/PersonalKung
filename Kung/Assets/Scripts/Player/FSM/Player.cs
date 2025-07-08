using TMPro.EditorUtilities;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private GroundChecker groundChecker;


    public Rigidbody2D rigid;
    public PlayerHealth playerHealth;
    public GameObject glowOutlineObj;

    public Animator bodyAnimator;
    public Animator headAnimator;
    public Animator boosterAnimator; 
    public Animator drillSideAnimator;
    public Animator drillDownAnimator;
    
    public SpriteRenderer headSpriteRanderer;
    public SpriteRenderer bodySpriteRanderer;
    public Sprite sideBodyDrillSprite;
    public Sprite frontHeadDrillSprite;
    public Sprite sideHeadDrillSprite;

    public Drilling drilling;
    public PlayerStats playerStats;

    public TileManager tileManager;
    [HideInInspector] public Tilemap brokenableTilemap;

    private StateMachine _moveStateMachine;
    private StateMachine _drillStateMachine;
    private int _drillDirection = 0;
    public const float maxFallSpeed = -4f;

    public Vector2 moveInput;
    public float[,] tiles = new float[0,0];
    [HideInInspector] public bool isBoost;
    [HideInInspector] public bool isDrillButtonDown;
    private bool _isDirectionMoving;


    private float _horizontal = 0f;

    public void OnLeftButtonDown() => _horizontal = -1f;
    public void OnRightButtonDown() => _horizontal = 1f;
    public void OnButtonUp() => _horizontal = 0f;

    void Start()
    {
        _moveStateMachine = new StateMachine();
        _drillStateMachine = new StateMachine();

        brokenableTilemap = tileManager.brokenableTilemap;
        tiles = tileManager.tiles;

        _moveStateMachine.ChangeState(new IdleState(this));
        _drillStateMachine.ChangeState(new DrillOffState(this));

        glowOutlineObj.SetActive(false);

}

void Update()
    {
        if (playerHealth.isDamaged)
        {
            return;
        }
        _moveStateMachine.Update();
        _drillStateMachine.Update();

        if (_isDirectionMoving) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow Pressed");
            isBoost = true;
            if (!(_moveStateMachine.CurrentState is BoostState))
                _moveStateMachine.ChangeState(new BoostState(this));
            _drillStateMachine.ChangeState(new DrillOffState(this));
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (!(_moveStateMachine.CurrentState is IdleState))
            _moveStateMachine.ChangeState(new IdleState(this));
        }
        

        // === 입력 처리 ===
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (!isBoost && !groundChecker.IsGrounded)
        {
            if (!(_moveStateMachine.CurrentState is FallingState))
            {
                _moveStateMachine.ChangeState(new FallingState(this));
            }   
            if (!(_drillStateMachine.CurrentState is DrillOffState))
            {
                _drillStateMachine.ChangeState(new DrillOffState(this));
            }
            return;
        }
        else
        {
            if (_moveStateMachine.CurrentState is FallingState)
            {
                _moveStateMachine.ChangeState(new IdleState(this));
            }
        }

        if (isBoost)
        {
            return;
        }

        // === 이동 FSM 상태 전이 ===
        if (moveInput.x != 0 && !(_moveStateMachine.CurrentState is RunState))
        {
            _moveStateMachine.ChangeState(new RunState(this));
        }
        else if (moveInput.x == 0 && !(_moveStateMachine.CurrentState is IdleState))
        {
            _moveStateMachine.ChangeState(new IdleState(this));
        }

        if (groundChecker.IsGrounded == false)
        {
            if (!(_drillStateMachine.CurrentState is DrillOffState))
            {
                _drillStateMachine.ChangeState(new DrillOffState(this));
            }
            return;
        }

        // === 드릴 FSM 상태 전이 ===
        if (Input.GetKey(KeyCode.X) || isDrillButtonDown)
        {
            _drillDirection = (int)moveInput.x;
            bool canDrillCurrentDirection = drilling.CanDrill(_drillDirection);

            if (canDrillCurrentDirection)
            {
                if (_drillDirection == 0) // 아래 드릴
                {
                    if (!(_drillStateMachine.CurrentState is DrillDownState))
                    {
                        _drillStateMachine.ChangeState(new DrillDownState(this));
                    }
                }
                else 
                {
                    if (!(_drillStateMachine.CurrentState is DrillSideState))
                    {
                        _drillStateMachine.ChangeState(new DrillSideState(this, _drillDirection));
                    }
                }
            }
            else 
            {
                if (!(_drillStateMachine.CurrentState is DrillOffState))
                {
                    _drillStateMachine.ChangeState(new DrillOffState(this));
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            if (!(_drillStateMachine.CurrentState is DrillOffState))
            {
                _drillStateMachine.ChangeState(new DrillOffState(this));
            }
        }

        
    }


    private void OnEnable()
    {
        Booster.OnBoostInput += HandleBoostInput;
        Booster.OnBoostRelease += HandleBoostRelease;
        DrillButton.OnDrillInput += HandleDrillInput;
        DrillButton.OnDrillRelease += HandleDrillRelease;
        Direction.OnLeftInput += HandleLeftInput;
        Direction.OnLeftRelease += HandleLeftRelease;
        Direction.OnRightInput += HandleRightInput;
        Direction.OnRightRelease += HandleRightRelease;
    }

    private void OnDisable()
    {
        Booster.OnBoostInput -= HandleBoostInput;
        Booster.OnBoostRelease -= HandleBoostRelease;
        DrillButton.OnDrillInput -= HandleDrillInput;
        DrillButton.OnDrillRelease -= HandleDrillRelease;
        Direction.OnLeftInput -= HandleLeftInput;
        Direction.OnLeftRelease -= HandleLeftRelease;
        Direction.OnRightInput -= HandleRightInput;
        Direction.OnRightRelease -= HandleRightRelease;
    }
    private void HandleBoostInput()
    {
        if (!(_moveStateMachine.CurrentState is BoostState))
            _moveStateMachine.ChangeState(new BoostState(this));
        _drillStateMachine.ChangeState(new DrillOffState(this));
    }
    private void HandleBoostRelease()
    {
        if (!(_moveStateMachine.CurrentState is IdleState))
            _moveStateMachine.ChangeState(new IdleState(this));
    }
    private void HandleDrillInput()
    {
        isDrillButtonDown = true;
    }
    private void HandleDrillRelease()
    {
        isDrillButtonDown = false;
    }


    private void HandleLeftInput()
    {
        if(!(_moveStateMachine.CurrentState is RunState))
        {
            moveInput.x = -1f;
            _moveStateMachine.ChangeState(new RunState(this));
            _isDirectionMoving = true;
        }
    }

    private void HandleRightInput()
    {
        if (!(_moveStateMachine.CurrentState is RunState))
        {
            moveInput.x = 1f;
            _moveStateMachine.ChangeState(new RunState(this));
            _isDirectionMoving = true;
        }
    }

    private void HandleLeftRelease()
    {
        if(!(_moveStateMachine.CurrentState is IdleState)){
            moveInput.x = 0f;
            _moveStateMachine.ChangeState(new IdleState(this));
            _isDirectionMoving = false;
        }
    }
    private void HandleRightRelease()
    {
        if (!(_moveStateMachine.CurrentState is IdleState))
        {
            moveInput.x = 0f;
            _moveStateMachine.ChangeState(new IdleState(this));
            _isDirectionMoving = false;
        }
    }


    // === 공용 기능 ===


    public void PlayDrillSideAnim(bool on)
    {
        drillSideAnimator.SetBool("Start", on);
    }

    public void PlayDrillDownAnim(bool on)
    {
        drillDownAnimator.SetBool("Start", on);
    }
   
}
