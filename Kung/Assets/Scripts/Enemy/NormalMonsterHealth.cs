using System.Collections;
using UnityEngine;

public class NormalMonsterHealth : MonoBehaviour
{
    public int hp;
    private int _maxHp = 30;

    public Animator animator;
    [SerializeField] private GameObject _treasureChest;

    const float TreasureChestOffset = 0.07f;
    const int DestroyTime = 2;

    public bool isDead;
    private void Awake()
    {
        hp = _maxHp;
    }

    private void Update()
    {
        if (isDead) return;

        // Test용 몬스터 죽이기
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(_maxHp);
        }
    }


    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;
        Debug.Log($"몬스터 현재 체력 : {hp}");
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");
        Debug.Log("일반몬스터 사망");

        Vector2 TreasureChestPosition = transform.position;
        TreasureChestPosition.y -= TreasureChestOffset;
        Instantiate(_treasureChest, TreasureChestPosition, Quaternion.identity);

        Destroy(gameObject, DestroyTime);
    }
}
