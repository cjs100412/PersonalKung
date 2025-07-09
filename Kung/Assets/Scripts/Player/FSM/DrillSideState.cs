using UnityEngine;

public class DrillSideState : IState
{
    private Player player;
    public int Direction { get; private set; }
    private int direction;
    private const float _drillDuration = 0.3f;
    private float _checkTime = 0f;

    public DrillSideState(Player player, int direction)
    {
        this.player = player;
        this.direction = direction;
    }

    public void Enter()
    {
        player.PlayDrillSideAnim(true);
        player.drilling.StartDrilling((int)player.moveInput.x);
        // 드릴 시작 시 이동 애니메이션 끄기
        //player.headAnimator.SetBool("Move", true);
        //player.bodyAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("isDrilling", true);

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
        player.PlayDrillSideAnim(false);
        player.drilling.StopDrilling();
        player.bodyAnimator.SetBool("isDrilling", false);


    }
}
