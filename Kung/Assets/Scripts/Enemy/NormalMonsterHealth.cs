using System.Collections;
using UnityEngine;

public class NormalMonsterHealth : MonoBehaviour
{
    public int hp;
    private int maxHp = 30;

    public Animator animator;
    [SerializeField] GameObject TreasureChest;

    const float TreasureChestOffset = 0.07f;
    const int DestroyTime = 2;

    public bool isDead;
    private void Awake()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if (isDead) return;

        // Test용 몬스터 죽이기
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(maxHp);
        }
    }


    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;
        Debug.Log($"몬스터 현재 체력 : {hp}");
        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");
        Debug.Log("일반몬스터 사망");

        Vector2 TreasureChestPosition = transform.position;
        TreasureChestPosition.y -= TreasureChestOffset;
        Instantiate(TreasureChest, TreasureChestPosition, Quaternion.identity);

        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}
