using System;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public static event Action OnLeftInput;
    public static event Action OnRightInput;
    public static event Action OnLeftRelease;
    public static event Action OnRightRelease;

    public void PressLeft() => OnLeftInput?.Invoke();
    //public void ReleaseLeft()
    //{
    //    OnLeftInput?.Invoke();
    //    OnLeftRelease?.Invoke();
    //}
    public void ReleaseLeft() => OnLeftRelease?.Invoke();
    public void ReleaseRight() => OnRightRelease?.Invoke();

    public void PressRight() => OnRightInput?.Invoke();
    //public void ReleaseRight()
    //{
    //    OnRightInput?.Invoke();
    //    OnRightRelease?.Invoke();
    //}
}
