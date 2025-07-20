using System.Collections;
using UnityEngine;

public interface IBossPattern
{
    bool CanExecute(BossController boss, Transform player);

    IEnumerator Execute(BossController boss, Transform player);

    float Cooldown { get; }

    void Reset();
}

public interface ISpawnPattern : IBossPattern
{
    void SetSpawnPoint(Transform spawnPoint);
}