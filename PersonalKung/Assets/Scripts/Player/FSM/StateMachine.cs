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
        CurrentState?.Exit();      // 이전 상태 나가기
        CurrentState = newState;
        CurrentState?.Enter();     // 새 상태 들어가기
    }

    public void Update()
    {
        CurrentState?.Update();    // 현재 상태 유지 중 처리
    }
}
