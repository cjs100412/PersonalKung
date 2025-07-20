using System;
using UnityEngine;

public class DrillButton : MonoBehaviour
{
    public static event Action OnDrillInput;
    public static event Action OnDrillRelease;

    public void PressDrill() => OnDrillInput?.Invoke();
    public void ReleaseDrill() => OnDrillRelease?.Invoke();
}
