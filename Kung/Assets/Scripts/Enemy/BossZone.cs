using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField] private GameObject _bossEnergy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _bossEnergy.SetActive(true);
            SoundManager.Instance.PlayBGM(BGM.bossbgm);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _bossEnergy.SetActive(false);
            SoundManager.Instance.PlayBGM(BGM.InGame);
        }
    }
}
