using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image hpImage;
    public TextMeshProUGUI hpText;
    public Image airBlueImage;
    public Image airYellowImage;
    public Image airRedImage;
    public Image airNeedle;
    public PlayerHealth playerHealth;
    public int score = 0;
    public TextMeshProUGUI scoreText;


    public Transform enterTarget;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private GameObject _playerDiePanel;

    private float _UIspeed = 0.5f;
    public void OnClickInventoryButton()
    {
        SoundManager.Instance.PlaySFX(SFX.InventoryOpen);
        _inventoryUI.transform.DOLocalMove(enterTarget.localPosition, _UIspeed);
        _inventoryUI.Refresh();
    }

    public void OnMenuButton()
    {
        _playerDiePanel.gameObject.SetActive(false);
        SceneManager.LoadScene("TitleScene");
    }
    public void OnLoadButton()
    {
        _playerDiePanel.gameObject.SetActive(false);
        SceneManager.LoadScene("LoadingScene");
    }

    private void Update()
    {
        scoreText.text = score.ToString();

        if (playerHealth.hp.Amount <= 0)
        {
            hpImage.fillAmount = 0f;
            hpText.text = "0 / 100";
        }
        else
        {
            hpImage.fillAmount = Mathf.InverseLerp(0f, 100f, playerHealth.hp.Amount);
            hpText.text = $"{playerHealth.hp.Amount} / 100";
        }

        float z = (playerHealth.air.Amount / (float)playerHealth.air.MaxAmount) * 180f;
        airNeedle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z - 90));

        if (playerHealth.air.Amount > playerHealth.air.MaxAmount / 2 )
        {
            airBlueImage.gameObject.SetActive(true);
            airYellowImage.gameObject.SetActive(false);
            airRedImage.gameObject.SetActive(false);
            airBlueImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.air.MaxAmount, playerHealth.air.Amount);
        }
        else if(playerHealth.air.Amount > playerHealth.air.Amount / 5)
        {
            airBlueImage.gameObject.SetActive(false);
            airYellowImage.gameObject.SetActive(true);
            airRedImage.gameObject.SetActive(false);
            airYellowImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.air.MaxAmount, playerHealth.air.Amount);
        }
        else
        {
            airBlueImage.gameObject.SetActive(false);
            airYellowImage.gameObject.SetActive(false);
            airRedImage.gameObject.SetActive(true);
            airRedImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.air.MaxAmount, playerHealth.air.Amount);
        }
    }
}
