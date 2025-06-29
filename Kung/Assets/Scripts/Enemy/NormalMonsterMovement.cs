using UnityEngine;

public class NormalMonster : MonoBehaviour
{
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private NormalMonsterHealth health;
    public float speed = 0.5f;
    public LayerMask groundLayer;

    private float checkDistance = 0.1f;
    private bool movingRight = true;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (health.isDead)
        {
            rigid.linearVelocity = Vector2.zero;
            return;
        }
        rigid.linearVelocity = new Vector2((movingRight ? 1 : -1) * speed, rigid.linearVelocity.y);

        Debug.DrawRay(GroundCheck.position, new Vector2(0, -checkDistance), new Color(1, 0, 0));
        bool isGroundAhead = Physics2D.Raycast(GroundCheck.position, Vector2.down, checkDistance, groundLayer);

        Debug.DrawRay(wallCheck.position, movingRight ? new Vector2(checkDistance, 0) : new Vector2(-checkDistance, 0), new Color(1, 0, 0));
        bool isWallAhead = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, checkDistance, groundLayer);

        if(!isGroundAhead || isWallAhead)
        {
            Flip();
        }
    }
    void Flip()
    {
        movingRight = !movingRight;

        // transform.localScale은 변수가 아니라서 바로 수정할 수가 없음; 그래서 변수하나 만듬
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
