using UnityEngine;

public class DrillOffState : IState
{
    private Player player;

    public DrillOffState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayDrillDownAnim(false);
        player.PlayDrillSideAnim(false);
        player.drilling.StopDrilling();
    }

    public void Update() { }

    public void Exit() { }
}
