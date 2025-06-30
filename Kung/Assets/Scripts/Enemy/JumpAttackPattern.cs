using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "JumpAttackPattern",
    menuName = "BossPatterns/Jump Attack (Max 40m, Only Above)",
    order = 12)]
public class JumpAttackPattern : ScriptableObject, IBossPattern
{
    public float maxDistance = 40f;
    public float cooldown = 5f;

    public float jumpForce = 12f;
    public float horizontalSpeed = 20f;
    public float gravityMultiplier = 3f;

    public Vector2 groundCheckOffset = new Vector2(0, -3f);
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    private float _lastUsedTime = -Mathf.Infinity;
    public float Cooldown => cooldown;

    private void OnEnable()
    {
        _lastUsedTime = -Mathf.Infinity;
    }

    public bool CanExecute(BossController boss, Transform player)
    {
        float dist = Vector2.Distance(boss.transform.position, player.position);
        bool isAbove = player.position.y > boss.transform.position.y;
        return Time.time >= _lastUsedTime + cooldown && dist <= maxDistance && isAbove;
    }

    public IEnumerator Execute(BossController boss, Transform player)
    {
        boss.isBusy = true;
        _lastUsedTime = Time.time;

        boss.animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.1f);

        Rigidbody2D rigid = boss.GetComponent<Rigidbody2D>();
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = originalGravity * gravityMultiplier;

        Vector2 toPlayer = player.position - boss.transform.position;
        float distance = toPlayer.magnitude;

        rigid.linearVelocity = new Vector2(horizontalSpeed, jumpForce);

        yield return new WaitForFixedUpdate();

        bool landed = false;
        // 최대 대기 시간을 설정, 땅에 떨어지지 않아도 최대 대기시간이 넘으면 탈출
        float timer = 0f, maxWait = 2f;
        while (timer < maxWait)
        {
            Vector2 checkPos = (Vector2)boss.transform.position + groundCheckOffset;
            if (Physics2D.OverlapCircle(checkPos, groundCheckRadius, groundLayer) != null)
            {
                landed = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // 최대 대기시간으로 빠져나오면 landed가 false임. 땅이 감지될때까지 대기
        if (!landed)
        {
            do { yield return null; }
            while (Physics2D.OverlapCircle((Vector2)boss.transform.position + groundCheckOffset, groundCheckRadius, groundLayer) == null);
        }

        rigid.gravityScale = originalGravity;
        yield return new WaitForSeconds(0.1f);

        boss.isBusy = false;
    }
}
