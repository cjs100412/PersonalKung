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
        // �帱 ���� �� �̵� �ִϸ��̼� ����
        player.headAnimator.enabled = false;
        player.bodyAnimator.enabled = false;
        player.bodySpriteRanderer.sprite = player.sideBodyDrillSprite;
        player.headSpriteRanderer.sprite = player.sideHeadDrillSprite;

    }

    public void Update()
    {
        player.drilling.ProcessDrilling();
    }

    public void Exit()
    {
        player.PlayDrillSideAnim(false);
        player.drilling.StopDrilling();
        player.bodyAnimator.enabled = true;
        player.headAnimator.enabled = true;


    }
}
