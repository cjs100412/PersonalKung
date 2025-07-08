using UnityEngine;

class FallingState : IState
{
    Player player;
    float dropDamageTime;
    public FallingState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("falling");
        dropDamageTime = 0;

    }

    public void Exit()
    {
        Debug.Log("exit falling");
        if (player.isBoost == false && dropDamageTime >= 0.5f)
        {
            Debug.Log("Player took fall damage");
            player.playerHealth.TakeDamage(20);
        }
    }

    public void Update()
    {
        dropDamageTime += Time.deltaTime;
        if (player.rigid.linearVelocity.y < Player.maxFallSpeed)
        {
            player.rigid.linearVelocity = new Vector2(player.rigid.linearVelocity.x, Player.maxFallSpeed);
        }
        player.rigid.linearVelocity = new Vector2(player.moveInput.x * player.playerStats.movementSpeed * 100 * Time.deltaTime, player.rigid.linearVelocity.y);
    }
}
