public class DrillSideState : IState
{
    private Player player;
    public int Direction { get; private set; }
    private int direction;

    public DrillSideState(Player player, int direction)
    {
        this.player = player;
        this.direction = direction;
    }

    public void Enter()
    {
        player.PlayDrillSideAnim(true);
        player.drilling.StartDrilling(direction);
        // 드릴 시작 시 이동 애니메이션 끄기
        player.headAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("Move", false);
        player.bodyAnimator.SetBool("isDrilling", true);

    }

    public void Update()
    {
        player.drilling.ProcessDrilling();
    }

    public void Exit()
    {
        player.PlayDrillSideAnim(false);
        player.drilling.StopDrilling();
        player.headAnimator.SetBool("Move", false);
        player.bodyAnimator.SetBool("isDrilling", false);


    }
}
