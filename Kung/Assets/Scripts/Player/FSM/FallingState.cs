using UnityEngine;

class FallingState : IState
{
    Player player;
    public FallingState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("falling");
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (player.rigid.linearVelocity.y < Player.maxFallSpeed)
        {
            player.rigid.linearVelocity = new Vector2(player.rigid.linearVelocity.x, Player.maxFallSpeed);
        }
        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * 100 * Time.deltaTime, player.rigid.linearVelocity.y);
    }
}
