using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "HomingMissilePattern", menuName = "BossPatterns/Homing Missile", order = 13)]
public class ArmMissilePattern : ScriptableObject, ISpawnPattern
{
    public GameObject missilePrefab;
    public float missileSpeed = 3f;
    public float turnSpeed = 200f;
    public int missileCount = 1;

    public float minDistance = 1f;
    public float maxDistance = 50f;
    public float cooldown = 3f;

    private Transform spawnPoint;
    float lastUsedTime = -Mathf.Infinity;
    public float Cooldown => cooldown;

    public void Reset()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    private void OnEnable()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    public void SetSpawnPoint(Transform sp)
    {
        spawnPoint = sp;
    }

    public bool CanExecute(BossController boss, Transform player)
    {
        float dist = Vector2.Distance(boss.transform.position, player.position);
        return Time.time >= lastUsedTime + cooldown && dist >= minDistance && dist <= maxDistance;
    }

    public IEnumerator Execute(BossController boss, Transform player)
    {
        lastUsedTime = Time.time;

        boss.animator.SetTrigger("Missile");

        yield return new WaitForSeconds(0.3f);

        GameObject proj = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        
        ArmMissile homing = proj.GetComponent<ArmMissile>();
        if (homing != null)
        {
            homing.Init(player, missileSpeed, turnSpeed);
        }

        yield return new WaitForSeconds(0.2f);
    }
}
