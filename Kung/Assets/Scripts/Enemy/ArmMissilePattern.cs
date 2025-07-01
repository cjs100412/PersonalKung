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

    private void OnEnable()
    {
        // 첫 실행에 쿨다운 검사에서 무조건 True가 나오게 하기 위한 초기화
        lastUsedTime = -Mathf.Infinity;
    }

    // ISpawnPattern상속 메소드.이 패턴의 spawnPoint에 넣을 Transform을 보스컨트롤러에서 받아온다
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

        //발사 모션과 맞추기 위해 약간 텀 주기
        yield return new WaitForSeconds(0.3f);

        GameObject proj = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        
        //ArmMissile 코드의 Init메소드를 호출해 초기화
        ArmMissile homing = proj.GetComponent<ArmMissile>();
        if (homing != null)
        {
            homing.Init(player, missileSpeed, turnSpeed);
        }

        yield return new WaitForSeconds(0.2f);
    }
}
