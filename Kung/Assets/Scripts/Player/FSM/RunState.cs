using UnityEngine;
public class RunState : IState
{
    //float direction;
    float _playerSpeed = 2;
    Player player;
    public RunState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {

        player._headAnimator.SetBool("Move", true);
        player._bodyAnimator.SetBool("Move", true);

    }
    public void Update()
    {
        if (player.isDrilling)
        {
            player._headAnimator.enabled = false;
            player._bodyAnimator.enabled = false;
        }
        else
        {
            player._headAnimator.enabled = true;
            player._bodyAnimator.enabled = true;
        }

        player.rigid.linearVelocity = new Vector2(player.currentDirection * _playerSpeed, player.rigid.linearVelocity.y);
        if (player.currentDirection == 1)
        {
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    public void Exit()
    {
        player.rigid.linearVelocity = new Vector2(0, player.rigid.linearVelocity.y);
        player._headAnimator.SetBool("Move", false);
        player._bodyAnimator.SetBool("Move", false);
        //player.InputLockState = InputLockState.Any;

    }
}
