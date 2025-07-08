// SummonPattern.cs
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SummonPattern", menuName = "BossPatterns/Summon", order = 15)]
public class SummonPattern : ScriptableObject, ISpawnPattern
{
    public GameObject summonPrefab;
    public float maxDistance = 20f;
    public float cooldown = 20f;          
    public int summonCount = 5;          
    public float spawnInterval = 0.1f;

    public float launchForce = 10f;
    public float angleRange = 90f;

    private Transform _spawnPoint;

    private float _lastUsedTime = -Mathf.Infinity;
    public float Cooldown => cooldown;

    private void OnEnable()
    {
        _lastUsedTime = -Mathf.Infinity;
    }

    public void SetSpawnPoint(Transform sp)
    {
        _spawnPoint = sp;
    }

    public bool CanExecute(BossController boss, Transform player)
    {
        float dist = Vector2.Distance(boss.transform.position, player.position);

        return Time.time >= _lastUsedTime + cooldown && dist <= maxDistance;
    }

    public IEnumerator Execute(BossController boss, Transform player)
    {
        _lastUsedTime = Time.time;

        boss.animator.SetTrigger("Summon");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < summonCount; i++)
        {
            var obj = Instantiate(summonPrefab, _spawnPoint.position, Quaternion.identity);

            // 랜덤 각도 계산
            float half = angleRange * 0.5f;
            float randomAngle = Random.Range(-half, half);

            Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;

            Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
            if(rigid != null)
            {
                rigid.linearVelocity = dir * launchForce;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(0.2f);
    }
}
