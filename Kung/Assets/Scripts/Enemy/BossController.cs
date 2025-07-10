using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour,IDamageable
{
    public Animator animator;
    public Transform player;
    public Transform spawner;
    [SerializeField] private GameObject _damageZone;
    [SerializeField] private Image _bossEnergyBar;

    public List<ScriptableObject> patternSOs;

    List<IBossPattern> patterns;
    public bool isBusy;

    const int DestroyTime = 5;

    [HideInInspector] public Health hp;
    private int _maxhp = 600;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private float _blinkDuration = 0.2f;
    void Awake()
    {
        hp = Health.New(_maxhp, _maxhp);

        patterns = new List<IBossPattern>();

        foreach (ScriptableObject so in patternSOs)
        {
            if (so is IBossPattern bossPattern)
            {
                IBossPattern newPatternInstance = Instantiate(so) as IBossPattern;
                if (newPatternInstance != null)
                {
                    patterns.Add(newPatternInstance);
                }
            }
        }
    }

    void OnEnable()
    {
        foreach (var pattern in patterns)
        {
            pattern.Reset();
        }
        isBusy = false;
    }

    public void ResetPatternCooldowns()
    {
        foreach (var pattern in patterns)
        {
            pattern.Reset();
        }
    }

    void Update()
    {
        if (hp.Amount <= 0)
        {
            _bossEnergyBar.fillAmount = 0f;
        }
        else
        {
            _bossEnergyBar.fillAmount = Mathf.InverseLerp(0f, _maxhp, hp.Amount);
        }
        if (isBusy)
        {
            return;
        }
        if (hp.IsDead) return;

        var tempList = new List<IBossPattern>();
        foreach (var pattern in patterns)
        {
            if (pattern.CanExecute(this, player))
            {
                tempList.Add(pattern);
            }
        }

        IBossPattern[] available = tempList.ToArray();

        if (available.Length == 0) return;

        foreach (IBossPattern pattern in available)
        {
            if (pattern is ISpawnPattern spawnPat)
                spawnPat.SetSpawnPoint(spawner);
        }

        var choice = available[Random.Range(0, available.Length)];
        StartCoroutine(RunPattern(choice));
    }
    private IEnumerator DamageBlink()
    {
        for (int i = 0; i < 3; i++)
        {
            Color color = _spriteRenderer.color;
            color = new Color(1, 1, 1, 0.5f);
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(_blinkDuration);

            color = new Color(1, 1, 1, 1);
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(_blinkDuration);
        }
    }
    IEnumerator RunPattern(IBossPattern pattern)
    {
        isBusy = true;
        yield return StartCoroutine(pattern.Execute(this, player));
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }
    public void TakeDamage(int amount)
    {
        if (hp.IsDead) return;

        hp = hp.TakeDamage(amount);

        StartCoroutine(DamageBlink());

        if (hp.IsDead)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("isDead");

        Destroy(_damageZone);
        Vector2 TreasureChestPosition = transform.position;

        Destroy(gameObject, DestroyTime);
        StartCoroutine(EndingScene());
    }

    IEnumerator EndingScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("EndingScene");
    }
}