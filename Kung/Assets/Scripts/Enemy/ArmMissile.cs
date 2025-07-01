using UnityEngine;

public class ArmMissile : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rigid;
    private float _speed;
    private float _turnSpeed;
    public int damage = 10;

    public GameObject explodeEffect;

    private float homingDuration = 1f;
    private float spawnTime;
    private bool stoppedHoming = false;
    private float destroyHoming = 5.0f;
    private float destroytime = 0f;

    // HomingMissilePattern코드에서 초기화하기 위한 메소드
    public void Init(Transform target, float speed, float turnSpeed)
    {
        this._target = target;
        this._speed = speed;
        this._turnSpeed = turnSpeed;
        _rigid = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        destroytime += Time.deltaTime;
        if ( destroytime > destroyHoming )
        {
            Destroy(gameObject);
            return;
        }

        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!stoppedHoming)
        {
            Vector2 dir = ((Vector2)_target.position - _rigid.position).normalized;

            // Vector3.Cross의 z값이 양수면 반시계, 음수면 시계 방향 회전.
            float rotateAmount = Vector3.Cross(dir, transform.up).z;

            // 리지드바디의 회전속도 설정. 
            _rigid.angularVelocity = -rotateAmount * _turnSpeed;

            if (Time.time - spawnTime >= homingDuration)
            {
                stoppedHoming = true;
                // homingDuration만큼의 시간이 지난 뒤, 추적, 회전을 멈추고 직진만
                _rigid.angularVelocity = 0f;
                _rigid.linearVelocity = transform.up * _speed;
            }
            else
            {
                _rigid.linearVelocity = transform.up * _speed;
            }
        }
        else
        {
            _rigid.linearVelocity = transform.up * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.hp = playerHealth.hp.TakeDamage(damage);

            GameObject effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(effect, 1f);
        }
    }
}
