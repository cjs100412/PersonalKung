using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    // 플레이어에 부착할 컴포넌트
    // 플레이어의 이동 제어

    public float speed;
    private Animator animator;
    [SerializeField] private SpriteRenderer spriteRendererHead;
    [SerializeField] private SpriteRenderer spriteRendererBody;
    


    [SerializeField] private Sprite frontHead;
    [SerializeField] private Sprite frontBody;

    [SerializeField] private Sprite sideHead;
    [SerializeField] private Sprite sideBody;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //animator.SetBool("Moving", true);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            spriteRendererHead.sprite = sideHead;
            spriteRendererBody.sprite = sideBody;
            spriteRendererHead.flipX = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //animator.SetBool("Moving", true);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            spriteRendererHead.sprite = sideHead;
            spriteRendererBody.sprite = sideBody;
            spriteRendererHead.flipX = true;

        }
        else
        {
            spriteRendererHead.sprite = frontHead;
            spriteRendererBody.sprite = frontBody;
            //animator.SetBool("Moving", false);
        }


    }
}
