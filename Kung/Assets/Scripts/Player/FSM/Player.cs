using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [Header("드릴 성능")]
    public float drillDamage;
    public float drillCoolTime;


    //private Coroutine _drillCoroutine;

    public Rigidbody2D rigid;
    public Animator _bodyAnimator;
    public Animator _headAnimator;
    public Animator _drillSide;
    public Animator _drillDown;
    public Drilling _drilling;
    public TileManager tileManager;

    public float[,] tiles = new float[0,0];
    public Sprite[] brokenTileSprites;
    public Tilemap brokenableTilemap;

    private StateMachine moveStateMachine;
    private StateMachine drillStateMachine;
    public InputLockState InputLockState = InputLockState.Any;
    public int currentDirection = 0;
    public bool isDrilling;
    void Start()
    {
        moveStateMachine = new StateMachine();
        drillStateMachine = new StateMachine();
        tiles = tileManager.tiles;
        brokenTileSprites = tileManager.brokenTileSprites;
        brokenableTilemap = tileManager.brokenabelTileMap;
        moveStateMachine.ChangeState(new IdleState());
    }


    void Update()
    {
        moveStateMachine.Update();
        drillStateMachine.Update();
        if (Input.GetKey(KeyCode.X))
        {
            if (drillStateMachine.currentState is DrillState)
            {
                return;
            }
            drillStateMachine.ChangeState(new DrillState(this));
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            drillStateMachine.ChangeState(new IdleState());
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = -1;
            if (moveStateMachine.currentState is RunState) return;
            moveStateMachine.ChangeState(new RunState(this));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = 1;

            if (moveStateMachine.currentState is RunState) return;
            moveStateMachine.ChangeState(new RunState(this));
        }

        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = 0;
            moveStateMachine.ChangeState(new IdleState());
        }
    }


}
