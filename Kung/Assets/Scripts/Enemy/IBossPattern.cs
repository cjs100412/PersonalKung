using System.Collections;
using UnityEngine;

public interface IBossPattern
{
    //패턴을 실행할 수 있는지 여부
    bool CanExecute(BossController boss, Transform player);

    //패턴 실행 코루틴
    IEnumerator Execute(BossController boss, Transform player);

    //쿨타임 시간 계산
    float Cooldown { get; }
}

public interface ISpawnPattern : IBossPattern
{
    // 프리팹 스폰을 사용하는 패턴들은 스폰포인트 설정도 해줘야함
    void SetSpawnPoint(Transform spawnPoint);
}