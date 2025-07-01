using DG.Tweening;
using System.Collections;
using UnityEngine;

public class InventoryUIOpen : MonoBehaviour
{
    public Transform enterTarget;
    public Transform quitTarget;

    private float _UIspeed = 0.5f;
    public void OnInventoryEnterButton()
    { 
        transform.DOLocalMove(enterTarget.localPosition, _UIspeed);
    }
    public void OnInventoryQuitButton()
    {
        transform.DOLocalMove(quitTarget.localPosition, _UIspeed);
    }
}
