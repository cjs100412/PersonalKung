using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ThreeSecondBOOM : MonoBehaviour
{
    [SerializeField] private GameObject _Effect;
    public int damage;

    void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnDestroy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.3f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D col in hits)
        {
            var damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        SoundManager.Instance.PlaySFX(SFX.Bomb);
        Destroy(Instantiate(_Effect, transform.position, Quaternion.identity), 3);
    }

    // 폭발 범위 보여주는 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
