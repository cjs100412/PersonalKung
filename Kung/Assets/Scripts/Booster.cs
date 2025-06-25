using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Booster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMovement playerMovement;
    public bool isBoost;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = playerMovement.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isBoost)
        {
            rb.AddForce(new Vector2(0,0), ForceMode2D.Force);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isBoost = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBoost = false;
    }


}
