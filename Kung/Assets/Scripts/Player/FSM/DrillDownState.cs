using UnityEngine;

public class DrillDownState : IState
{
    private Player player;
    private const float _drillDuration = 0.3f;
    private float _checkTime = 0f;
    public DrillDownState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayDrillDownAnim(true);
        player.drilling.StartDrilling(0);
        // 드릴 시작 시 이동 애니메이션 끄기
        //player.headAnimator.enabled = false;
        player.headAnimator.SetBool("isDrilling", true);
        //player.headSpriteRanderer.sprite = player.frontHeadDrillSprite;
        player.bodyAnimator.SetBool("Move", false);
    }

    public void Update()
    {
        _checkTime += Time.deltaTime;
        if (_checkTime > _drillDuration)
        {
            SoundManager.Instance.PlaySFX(SFX.Drilling);
            _checkTime = 0f;
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
