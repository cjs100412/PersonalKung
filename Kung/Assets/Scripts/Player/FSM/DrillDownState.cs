using Unity.Cinemachine;
using UnityEngine;

public class DrillDownState : IState
{
    private Player player;
    private const float interval = 0.3f;
    private float timer = 0f;
    public DrillDownState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayDrillDownAnim(true);
        player.drilling.StartDrilling(0);
        SoundManager.Instance.PlaySFX(SFX.Drilling);
        player.impulseSource.GenerateImpulse();
        // 드릴 시작 시 이동 애니메이션 끄기
        //player.headAnimator.enabled = false;
        player.headAnimator.SetBool("isDrilling", true);
        //player.headSpriteRanderer.sprite = player.frontHeadDrillSprite;
        player.bodyAnimator.SetBool("Move", false);
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            SoundManager.Instance.PlaySFX(SFX.Drilling);
            player.impulseSource.GenerateImpulse();
            timer = 0f;
        }
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
