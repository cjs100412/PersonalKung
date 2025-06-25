using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Booster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float boostSpeed;
    public PlayerMovement playerMovement;
    //Animator animator;
    Rigidbody2D rb;
    private bool isBoost;

    private void Awake()
    {
        //animator = playerMovement.GetComponent<Animator>();
        rb = playerMovement.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isBoost)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, boostSpeed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //animator.SetBool("isBoost", true);
        isBoost = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //animator.SetBool("isBoost", false);
        isBoost = false;
    }


}
