using System;
using UnityEngine;

public class Booster : MonoBehaviour
{
    // static으로 구현한 이벤트. 다른 클래스에서 구독 가능함.
    public static event Action<bool> OnBoostInput;

    // 이벤트트리거를 버튼에 달아서 포인터다운 감지하면 PressBoost 함수 실행. 대리자에 true를 전달함.( 눌렸다는 의미 )
    public void PressBoost() => OnBoostInput?.Invoke(true);
    // 이벤트트리거를 버튼에 달아서 포인터다운 감지하면 ReleaseBoost 함수 실행. 대리자에 false 전달함.( 뗐다는 의미 )
    public void ReleaseBoost() => OnBoostInput?.Invoke(false);
}
