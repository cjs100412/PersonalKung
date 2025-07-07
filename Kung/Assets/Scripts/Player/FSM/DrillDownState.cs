using UnityEngine;

public class DrillDownState : IState
{
    private Player player;

    public DrillDownState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayDrillDownAnim(true);
        player.drilling.StartDrilling(0);
        // �帱 ���� �� �̵� �ִϸ��̼� ����
        //player.headAnimator.enabled = false;
        player.headAnimator.SetBool("isDrilling", true);
        //player.headSpriteRanderer.sprite = player.frontHeadDrillSprite;
        player.bodyAnimator.SetBool("Move", false);
    }

    public void Update()
    {
        player.drilling.ProcessDrilling();

    }

    public void Exit()
    {
        player.PlayDrillDownAnim(false);
        player.headAnimator.SetBool("isDrilling", false);
        player.drilling.StopDrilling();
        //player.headAnimator.enabled = true;
    }
}
