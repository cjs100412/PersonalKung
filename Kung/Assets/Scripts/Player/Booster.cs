using System;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public static event Action OnBoostInput;
    public static event Action OnBoostRelease;

    public void PressBoost() => OnBoostInput?.Invoke();
    public void ReleaseBoost() => OnBoostRelease?.Invoke();
}
