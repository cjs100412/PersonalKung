using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int _maxair = 100;
    private int _maxhp = 100;
    private bool _isAirDecrease;
    private bool _isHpDecrease;

    private const float _decreaseAirTime = 3.0f;
    private const float _hpDecreaseInterval = 0.2f;

    private bool _isInvincible;
    private float _invincibleTime = 1.0f;

    [Header("Head랑 Body 애니메이터 연결")]
    public Animator headAnimator;
    public Animator bodyAnimator;

    public Health hp;
    public Air air;

    public int MaxHp => _maxhp;
    public int MaxAir
    {
        get
        {
            return _maxair;
        }
        private set
        {
            _maxair = value;
        }
    }

    private void Awake()
    {
        hp = Health.New(_maxhp, _maxhp);
        air = Air.New(_maxair, _maxair);
    }

    void Update()
    {
        if (hp.IsDead) return;

        if(transform.position.y >= -1 && air.Current < _maxair)
        {
            air = air.Heal(_maxair);
        }

        if(transform.position.y < -1 && !_isAirDecrease)
        {
            StartCoroutine(airDecrease());
        }

        if(air.IsAirZero && !_isHpDecrease)
        {
            StartCoroutine(hpDecrease());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            air = air.AirDecrease(5);
            Debug.Log($"현재 산소 :{air.Amount}");
        }
    }

    IEnumerator hpDecrease()
    {
        _isHpDecrease = true;
        yield return new WaitForSeconds(_hpDecreaseInterval);
        hp = hp.TakeDamage(1);
        Debug.Log($"현재 체력 : {hp.Amount}");

        if (hp.IsDead)
        {
            Die();
        }
        _isHpDecrease = false;
    }

    public void TakeDamage(int amount)
    {
        if (hp.IsDead || _isInvincible) return;

        hp = hp.TakeDamage(amount);
        if(hp.IsDead)
        {
            Die();
            return;
        }
        headAnimator.SetTrigger("isDamaged");
        bodyAnimator.SetTrigger("isDamaged");
        StartCoroutine(Invincible());
        Debug.Log($"현재 체력 : {hp.Amount}");
    }

    public void Die()
    {
        headAnimator.SetTrigger("isDead");
        bodyAnimator.SetTrigger("isDead");
        Debug.Log("사망");
    }

    IEnumerator Invincible()
    {
        _isInvincible = true;
        Debug.Log("무적");
        yield return new WaitForSeconds(_invincibleTime);
        _isInvincible = false;
    }
    
    IEnumerator airDecrease()
    {
        _isAirDecrease = true;
        air = air.AirDecrease(1);
        Debug.Log($"현재 산소 :{air.Amount}");
        yield return new WaitForSeconds(_decreaseAirTime);
        _isAirDecrease = false;
    }
}
