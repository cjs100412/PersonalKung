using UnityEngine;

public class RunState : IState
{
    float _playerSpeed = 2;
    Player player;

    public RunState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // �̵� �ִϸ��̼Ǹ� ����
        player.headAnimator.SetBool("Move", true);
        player.bodyAnimator.SetBool("Move", true);
        player.PlayDrillSideAnim(false); // �̵� ���� �� �帱 �ִϸ��̼� ����
        player.PlayDrillDownAnim(false); // �̵� ���� �� �帱 �ִϸ��̼� ����
    }

    public void Update()
    {
        // �帱 Ȱ��ȭ ���ο� ���� �̵� �ִϸ��̼��� �����ϴ� ������ ���� (�� �帱 ���¿��� �̵� �ִϸ��̼��� ���� ���� �� ����)

        player.rigid.linearVelocity = new Vector2(player.moveInput.x * _playerSpeed, player.rigid.linearVelocity.y);

        // ȸ�� ������ Player Ŭ������ ������ �̵� �������� ó���ϴ� ���� ����
        if (player.moveInput.x > 0)
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (player.moveInput.x < 0)
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Exit()
    {
        player.rigid.linearVelocity = new Vector2(0, player.rigid.linearVelocity.y);
        player.headAnimator.SetBool("Move", false);
        player.bodyAnimator.SetBool("Move", false);
    }
}