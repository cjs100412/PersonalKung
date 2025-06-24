using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Booster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMovement playerMovement;
    Animator[] animator;

    private void Awake()
    {
        //animator = playerMovement.GetComponentInChildren<Animator>();
        animator = playerMovement.GetComponentsInChildren<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Animator anim in animator)
        {
            anim.SetBool("isBoost", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Animator anim in animator)
        {
            anim.SetBool("isBoost", false);
        }
    }


}
