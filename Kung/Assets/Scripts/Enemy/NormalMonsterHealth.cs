using System.Collections;
using UnityEngine;

public class NormalMonsterHealth : MonoBehaviour
{
    
    private int _maxHp = 30;

    public Health hp;

    public Animator animator;
    [SerializeField] private GameObject _treasureChest;

    const float TreasureChestOffset = 0.07f;
    const int DestroyTime = 2;

    private void Awake()
    {
        hp = Health.New(_maxHp, _maxHp);
    }

    private void Update()
    {
        if (hp.IsDead) return;

        // Test용 몬스터 죽이기
        if (Input.GetKeyDown(KeyCode.P))
        {
            hp = hp.TakeDamage(_maxHp);
        }
    }


    public void TakeDamage(int amount)
    {
        if (hp.IsDead) return;

        hp = hp.TakeDamage(amount);
        Debug.Log($"몬스터 현재 체력 : {hp}");
        if (hp.IsDead)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("isDead");
        Debug.Log("일반몬스터 사망");

        Vector2 TreasureChestPosition = transform.position;
        TreasureChestPosition.y -= TreasureChestOffset;
        Instantiate(_treasureChest, TreasureChestPosition, Quaternion.identity);

        Destroy(gameObject, DestroyTime);
    }
}
