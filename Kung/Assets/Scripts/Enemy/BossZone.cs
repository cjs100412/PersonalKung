using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField] private GameObject _bossEnergy;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private BossController _bossController;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_bossController != null)
            {
                _bossController.ResetPatternCooldowns();
            }

            isActive = true;
            _bossEnergy.SetActive(true);
            SoundManager.Instance.PlayBGM(BGM.bossbgm);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerHealth.hp.IsDead)
            {
                isActive = false;
                _bossEnergy.SetActive(false);
                SoundManager.Instance.PlayBGM(BGM.InGame);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isActive) return;
            _bossEnergy.SetActive(false);
            SoundManager.Instance.PlayBGM(BGM.InGame);
        }
    }
}
