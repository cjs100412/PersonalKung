using System;
using System.Collections;
using UnityEngine;

// TODO : 낙뎀 추가 : IsGround 마지막으로 감지했을때가 언제인지 부스트 마지막으로 썼을때가 언제인지 계산

// TODO : 애니메이터 연결 : 맞는 모션, 죽는 모션. Trigger로 구현

// TODO : UI랑 연결 : HUD의 산소통과 체력통, 숫자랑 연결하기. UI전용 스크립트 필요함
public class PlayerHealth : MonoBehaviour
{
    [Header("체력, 산소")]
    public int air;
    public int hp;
    public int maxair = 100;
    private int maxhp = 100;
    private bool isAirDecrease;
    private bool isHpDecrease;


    private float decreaseAirTime = 3.0f;


    private bool isDead;
    
    
    private bool isInvincible;
    private float invincibleTime = 1.0f;

    [Header("Head랑 Body 애니메이터 연결")]
    public Animator headAnimator;
    public Animator bodyAnimator;

    private void Awake()
    {
        hp = maxhp;
        air = maxair;
    }

    void Update()
    {
        if (isDead) return;

        if(transform.position.y >= -4)
        {
            air = maxair;
        }

        // 일단 대충 0 아래로가면 산소가 달도록
        if(transform.position.y < -4 && !isAirDecrease)
        {
            StartCoroutine(airDecrease());
        }

        if(air <= 0 && !isHpDecrease)
        {
            StartCoroutine(hpDecrease());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            air -= 5;
            Debug.Log($"현재 산소 :{air}");
        }
    }

    IEnumerator hpDecrease()
    {
        isHpDecrease = true;
        yield return new WaitForSeconds(0.2f);
        hp--;
        Debug.Log($"현재 체력 : {hp}");

        if (hp <= 0)
        {
            Die();
        }
        isHpDecrease = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;
        headAnimator.SetTrigger("isDamaged");
        bodyAnimator.SetTrigger("isDamaged");
        StartCoroutine(Invincible());
        Debug.Log($"현재 체력 : {hp}");
        if(hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        headAnimator.SetTrigger("isDead");
        bodyAnimator.SetTrigger("isDead");
        Debug.Log("사망");
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        Debug.Log("무적");
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    
    IEnumerator airDecrease()
    {
        isAirDecrease = true;
        yield return new WaitForSeconds(decreaseAirTime);
        if(air >= 0)
        {
            air--;
            Debug.Log($"현재 산소 : {air}");
        }
        isAirDecrease = false;
    }
}
