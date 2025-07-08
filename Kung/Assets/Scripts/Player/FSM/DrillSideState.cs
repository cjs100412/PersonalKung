using Unity.Cinemachine;
using UnityEngine;

public class DrillSideState : IState
{
    private Player player;
    public int Direction { get; private set; }
    private int direction;
    public float interval = 0.3f;

    private float timer = 0f;
    public DrillSideState(Player player, int direction)
    {
        this.player = player;
        this.direction = direction;
    }

    public void Enter()
    {
        player.PlayDrillSideAnim(true);
        player.drilling.StartDrilling(direction);
        SoundManager.Instance.PlaySFX(SFX.Drilling);
        player.impulseSource.GenerateImpulse();
        // 드릴 시작 시 이동 애니메이션 끄기
        //player.headAnimator.SetBool("Move", true);
        //player.bodyAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("isDrilling", true);

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
        player.PlayDrillSideAnim(false);
        player.drilling.StopDrilling();
        player.bodyAnimator.SetBool("isDrilling", false);


    }
}
