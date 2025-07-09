using UnityEngine;

public class IdleState : IState
{
    private Player player; 

    public IdleState(Player player) 
    {
        this.player = player;
    }

    public void Enter()
    {
        player.headAnimator.SetBool("Move", false); 
        player.bodyAnimator.SetBool("Move", false);
        player.rigid.linearVelocity = new Vector2(0, player.rigid.linearVelocity.y);

    }
    public void Update() { }
    public void Exit() {  }
}