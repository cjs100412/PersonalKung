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
            var enemy = col.GetComponent<NormalMonsterHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        Destroy(Instantiate(_Effect, transform.position, Quaternion.identity), 3);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
