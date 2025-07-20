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
        player.headAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("Move", true);
    }

    public void Update()
    {

        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * Time.deltaTime * 70, player.rigid.linearVelocity.y);

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