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
    }
    public void Update() { }
    public void Exit() { Debug.Log("Idle Á¾·á"); }
}