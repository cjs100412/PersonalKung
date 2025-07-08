using UnityEngine;

public class RunState : IState
{
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
        //player.PlayDrillSideAnim(false); // 이동 시작 시 드릴 애니메이션 끄기
        //player.PlayDrillDownAnim(false); // 이동 시작 시 드릴 애니메이션 끄기
    }

    public void Update()
    {
        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * Time.deltaTime * 100, player.rigid.linearVelocity.y);
        Debug.Log("Velocity: " + player.rigid.linearVelocity);
        //player.gameObject.transform.Translate(new Vector2(player.moveInput.x * player.playerStats.movementSpeed * Time.deltaTime, 0),Space.World);
        if (player.moveInput.x > 0)
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (player.moveInput.x < 0)
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Exit()
    {
        player.headAnimator.SetBool("Move", false);
        player.bodyAnimator.SetBool("Move", false);
    }
}