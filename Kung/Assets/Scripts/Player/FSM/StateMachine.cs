using Unity.VisualScripting;
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    public IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();      // 이전 상태 나가기
        currentState = newState;
        currentState?.Enter();     // 새 상태 들어가기
    }

    public void Update()
    {
        currentState?.Update();    // 현재 상태 유지 중 처리
    }
}
