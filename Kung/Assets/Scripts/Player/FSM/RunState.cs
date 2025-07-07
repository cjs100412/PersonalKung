using UnityEngine;

public class RunState : IState
{
    float _playerSpeed = 2;
    Player player;

    public RunState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // 이동 애니메이션만 제어
        player.headAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("Move", true);
        player.PlayDrillSideAnim(false); // 이동 시작 시 드릴 애니메이션 끄기
        player.PlayDrillDownAnim(false); // 이동 시작 시 드릴 애니메이션 끄기
    }

    public void Update()
    {
        // 드릴 활성화 여부에 따라 이동 애니메이션을 제어하는 로직은 제거 (각 드릴 상태에서 이동 애니메이션을 끄는 것이 더 적절)

        player.rigid.linearVelocity = new Vector2(player.moveInput.x * _playerSpeed, player.rigid.linearVelocity.y);

        // 회전 로직은 Player 클래스나 별도의 이동 로직에서 처리하는 것을 권장
        if (player.moveInput.x > 0)
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (player.moveInput.x < 0)
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Exit()
    {
        player.rigid.linearVelocity = new Vector2(0, player.rigid.linearVelocity.y);
        player.headAnimator.SetBool("Move", false);
        player.bodyAnimator.SetBool("Move", false);
    }
}