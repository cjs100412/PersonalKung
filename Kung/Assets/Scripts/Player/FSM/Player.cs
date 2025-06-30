using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigid;
    [SerializeField] public Animator _bodyAnimator;
    [SerializeField] public Animator _headAnimator;
    [SerializeField] public Animator _drillSide;
    [SerializeField] public Animator _drillDown;
    [SerializeField] public Drilling _drilling;
    private StateMachine stateMachine;
    public InputLockState InputLockState = InputLockState.Any;
    bool isMoving;
    public int currentDirection = 0;
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState());
    }


    void Update()
    {
        stateMachine.Update();
        //InputLockState = currentDirection == 1 ? InputLockState.Left : InputLockState.Right;
        
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = -1;
            if (stateMachine.currentState is RunState) return;
            //InputLockState = InputLockState.Right;
            stateMachine.ChangeState(new RunState(this));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = 1;

            if (stateMachine.currentState is RunState) return;
            //InputLockState = InputLockState.Left;
            stateMachine.ChangeState(new RunState(this));
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (stateMachine.currentState is SideDrillState) return;
            stateMachine.ChangeState(new SideDrillState(this));

        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = 0;
            stateMachine.ChangeState(new IdleState());
        }
    }


}
