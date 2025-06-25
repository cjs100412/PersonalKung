using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Booster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMovement playerMovement;
    Animator[] animator;
    private bool isBoost;
    Rigidbody2D rb;

    private void Awake()
    {
        animator = playerMovement.GetComponentsInChildren<Animator>();
        rb = playerMovement.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isBoost)
        {
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, 1);
            rb.AddForce(Vector2.up * 2, ForceMode2D.Force);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isBoost = true;

        foreach (Animator anim in animator)
        {
            anim.SetBool("isBoost", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBoost = false;

        foreach (Animator anim in animator)
        {
            anim.SetBool("isBoost", false);
        }
    }


}
