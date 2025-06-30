using UnityEngine;

public class IdleState : IState
{
    //private Animator _headAnimator;
    //private Animator _bodyAnimator;
    //public IdleState(Animator head, Animator body)
    //{
    //    _headAnimator = head;
    //    _bodyAnimator = body;
    //}

    public void Enter()
    {
        //_headAnimator.SetBool("Move", false);
        //_bodyAnimator.SetBool("Move", false);
    }
    public void Update() { /* 입력 들어오면 달리기로 전이 예정 */ }
    public void Exit() { Debug.Log("Idle 종료"); }
}

public class RunState : IState
{
    //float direction;
    float _playerSpeed = 2;
    Player player;
    public RunState(Player player)
    {
        this.player = player;
    }

    public void Enter() 
    {

        player._headAnimator.SetBool("Move", true);
        player._bodyAnimator.SetBool("Move", true);
        
    }
    public void Update() 
    {
        player.rigid.linearVelocity = new Vector2(player.currentDirection * _playerSpeed, player.rigid.linearVelocity.y);
        if (player.currentDirection == 1)
        {
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    public void Exit() 
    {
        player.rigid.linearVelocity = new Vector2(0, player.rigid.linearVelocity.y);
        player._headAnimator.SetBool("Move", false);
        player._bodyAnimator.SetBool("Move", false);
        //player.InputLockState = InputLockState.Any;

    }
}

public class DownDrillState : IState
{
    private Drilling _drilling;
    private int direction;
    [SerializeField] private Animator _drillSide;
    [SerializeField] private Animator _drillDown;
    public DownDrillState(int dir, Drilling drilling, Animator drillSide, Animator drillDown)
    {
        direction = dir;
        _drilling = drilling;
        _drillSide = drillSide;
        _drillDown = drillDown;

    }

    public void Enter()
    {
        _drilling.C_StartDrilling(direction);
        
        _drillDown.SetBool("Start", true);

    }

    public void Exit()
    {
        _drilling.C_StopDrilling(direction);
        _drillDown.SetBool("Start", false);

    }

    public void Update()
    {
    }
}

public class SideDrillState : IState
{
    Player player;
    public SideDrillState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player._drilling.C_StartDrilling(player.currentDirection);
        player._drillSide.SetBool("Start", true);
        
    }

    public void Exit()
    {
        player._drilling.C_StopDrilling(player.currentDirection);
        player._drillSide.SetBool("Start", false);
    }

    public void Update()
    {
    }
}