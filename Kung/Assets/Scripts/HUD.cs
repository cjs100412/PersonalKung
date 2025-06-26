using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image hpImage;
    public TextMeshProUGUI hpText;
    public Image airBlueImage;
    public Image airYellowImage;
    public Image airRedImage;
    public Image airNeedle;
    public Image InventoryUI;
    public PlayerHealth playerHealth;

    private void Update()
    {
        hpImage.fillAmount = Mathf.InverseLerp(0f, 100f, playerHealth.hp);
        hpText.text = $"{playerHealth.hp} / 100";
        float z = (playerHealth.air / (float)playerHealth.maxair) * 180f;
        airNeedle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z - 90));


        if (playerHealth.air > playerHealth.maxair / 2 )
        {
            airBlueImage.gameObject.SetActive(true);
            airYellowImage.gameObject.SetActive(false);
            airRedImage.gameObject.SetActive(false);
            airBlueImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.maxair, playerHealth.air);
        }
        else if(playerHealth.air > playerHealth.maxair / 5)
        {
            airBlueImage.gameObject.SetActive(false);
            airYellowImage.gameObject.SetActive(true);
            airRedImage.gameObject.SetActive(false);
            airYellowImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.maxair, playerHealth.air);
        }
        else
        {
            airBlueImage.gameObject.SetActive(false);
            airYellowImage.gameObject.SetActive(false);
            airRedImage.gameObject.SetActive(true);
            airRedImage.fillAmount = Mathf.InverseLerp(0f, playerHealth.maxair, playerHealth.air);
        }
    }

    public void OnInventoryButton()
    {            InventoryUI.transform.localPosition = Vector3.Lerp(InventoryUI.transform.localPosition,
                                                           new Vector3(1035, 0, 0),
                                                           Time.deltaTime * 10);
    }
}
