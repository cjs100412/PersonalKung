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
        player.glowOutlineObj.SetActive(true);
        player.boosterAnimator.SetBool("isBoost", true);
    }

    public void Exit()
    {
        player.isBoost = false;
        player.glowOutlineObj.SetActive(false);
        player.boosterAnimator.SetBool("isBoost", false);

    }

    public void Update()
    {
        if (player.rigid.linearVelocity.y < player.playerStats.boosterSpeed / 3)
        {
            player.rigid.AddForce(Vector2.up * player.playerStats.boosterSpeed * Time.deltaTime * 50 , ForceMode2D.Force);
        }
        player.glowOutlineObj.transform.rotation = Quaternion.identity;
        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * 50 * Time.deltaTime, player.rigid.linearVelocity.y);
    }
}
