using UnityEngine;

public class NormalMonster : MonoBehaviour
{
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private NormalMonsterHealth _health;
    public float speed = 0.5f;
    public LayerMask groundLayer;
    public LayerMask rockLayer;
    public LayerMask groundAndRockLayer => groundLayer | rockLayer;

    private float _checkDistance = 0.1f;
    private bool _movingRight = true;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_health.hp.IsDead)
        {
            rigid.linearVelocity = Vector2.zero;
            return;
        }

        if (_movingRight)
        {
            rigid.linearVelocity = new Vector2(1 * speed, rigid.linearVelocity.y);
        }
        else
        {
            rigid.linearVelocity = new Vector2(-1 * speed, rigid.linearVelocity.y);
        }

        
        bool isGroundAhead = Physics2D.Raycast(_groundCheck.position, Vector2.down, _checkDistance, groundAndRockLayer);

        bool isWallAhead;

        if (_movingRight)
        {
            isWallAhead = Physics2D.Raycast(_wallCheck.position, Vector2.right, _checkDistance, groundAndRockLayer);
        }
        else
        {
            isWallAhead = Physics2D.Raycast(_wallCheck.position, Vector2.left, _checkDistance, groundAndRockLayer);
        }


        if(!isGroundAhead || isWallAhead)
        {
            Flip();
        }
    }
    void Flip()
    {
        _movingRight = !_movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
