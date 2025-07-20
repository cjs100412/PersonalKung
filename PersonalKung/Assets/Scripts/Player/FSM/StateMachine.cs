using Unity.VisualScripting;
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    public IState CurrentState;

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();      // ���� ���� ������
        CurrentState = newState;
        CurrentState?.Enter();     // �� ���� ����
    }

    public void Update()
    {
        CurrentState?.Update();    // ���� ���� ���� �� ó��
    }
}
