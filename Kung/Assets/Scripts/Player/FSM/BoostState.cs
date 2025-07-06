using UnityEngine;

public class BoostState : IState
{
    private Player player;
    public BoostState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.isBoost = true;
        player.boosterAnimator.SetBool("isBoost", true);
    }

    public void Exit()
    {
        player.isBoost = false;
        player.boosterAnimator.SetBool("isBoost", false);

    }

    public void Update()
    {
        player.rigid.AddForce(Vector2.up * player.playerStats.boosterSpeed * Time.deltaTime * 100, ForceMode2D.Force);
        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * 100 * Time.deltaTime, player.rigid.linearVelocity.y);
    }
}
