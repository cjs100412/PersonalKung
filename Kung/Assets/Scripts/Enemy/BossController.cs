using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BossController : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public Transform spawner;

    [Header("패턴 ScriptableObject")]
    public List<ScriptableObject> patternSOs;

    List<IBossPattern> patterns;
    public bool isBusy;

    Health health;
    private int _maxhp = 300;

    void Awake()
    {
        health = Health.New(_maxhp, _maxhp);

        // SO리스트에서 IBossPattern 인터페이스를 구현한것들만 patterns에 넣는다
        patterns = new List<IBossPattern>();

        foreach (ScriptableObject so in patternSOs)
        {
            if (so is IBossPattern bossPatterns)
            {
                patterns.Add(bossPatterns);
            }
        }
    }

    void Update()
    {
        if (isBusy)
        {
            return;
        }
         if (health.IsDead) return;

        var tempList = new List<IBossPattern>();
        //패턴들 중 실행 가능한 패턴들만을 리스트에 담고 배열로 변환한다
        foreach (var pattern in patterns)
        {
            if (pattern.CanExecute(this, player))
            {
                tempList.Add(pattern);
            }
        }

        IBossPattern[] available = tempList.ToArray();

        if (available.Length == 0) return;

        // 실행 가능한 패턴들 중, ISpawnPattern이 있으면 스폰 위치를 설정해준다
        foreach (IBossPattern pattern in available)
        {
            if (pattern is ISpawnPattern spawnPat)
                spawnPat.SetSpawnPoint(spawner);
        }

        // 무작위로 패턴 하나 선택 후 실행
        var choice = available[Random.Range(0, available.Length)];
        StartCoroutine(RunPattern(choice));
    }
    IEnumerator RunPattern(IBossPattern pattern)
    {
        isBusy = true;
        yield return StartCoroutine(pattern.Execute(this, player));
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }
}