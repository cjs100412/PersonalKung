using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleManager : MonoBehaviour
{
    private string _savePath;
    [SerializeField] private Image _newGameDialog;
    [SerializeField] private Image _noSaveFileDialog;
    [SerializeField] private Image _gameRuleDialog;
    [SerializeField] private Image _settingDialog;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void OnOkayButtonClick()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(_savePath))
            File.Delete(_savePath);

        SceneManager.LoadScene("OpeningScene");
    }

    public void OnGameLoadButtonClick()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(_savePath))
        {
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            _noSaveFileDialog.gameObject.SetActive(true);
        }
    }

    public void OnCloseButtonClick()
    {
        _newGameDialog.gameObject.SetActive(false);
    }

    public void OnNewGameButtonClick()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(_savePath))
        {
            _newGameDialog.gameObject.SetActive(true);
        }
        else
        {
            File.Delete(_savePath);
            SceneManager.LoadScene("OpeningScene");
        }
    }

    public void OnNoSaveFileCloseButtonClick()
    {
        _noSaveFileDialog.gameObject.SetActive(false);
    }
    public void OnGameRuleButtonClick()
    {
        _gameRuleDialog.gameObject.SetActive(true);
    }
    public void OnGameRuleCloseButtonClick()
    {
        _gameRuleDialog.gameObject.SetActive(false);
    }
    public void OnSettingButtonClick()
    {
        _settingDialog.gameObject.SetActive(true);
    }
    public void OnSettingCloseButtonClick()
    {
        _settingDialog.gameObject.SetActive(false);
    }
}
