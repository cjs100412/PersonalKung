using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private BossController _bossController;

    public float speed = 0.5f;


    void Update()
    {
        //if (_bossController.isBusy) return;

        Vector2 dist = _playerTransform.position - transform.position;

        Vector2 vel = rigid.linearVelocity;
        vel.x = dist.x * speed;

        rigid.linearVelocity = vel;

        float scaleX = (_playerTransform.position.x > transform.position.x) ? -1f : 1f;
        Vector2 s = transform.localScale;
        s.x = scaleX;
        transform.localScale = s;
    }


}
