using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [Header("Pause UI Panel")]
    [Tooltip("ESC ������ �� ���� UI Panel")]
    public GameObject pauseCanvas;


    void Start()
    {
        // ������ �� �ݵ�� ���α�
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    public void OnSettingButton()
    {
        PauseGame();
    }

    public void OnResumeButton()
    {
        ResumeGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        // Audio�� �Բ� ���߱�
        //AudioListener.pause = true;
        // UI ���̱�
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        // Audio�� ���
        //AudioListener.pause = false;
        // UI �����
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    public void OnTitleButton()
    {
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        SceneManager.LoadScene("TitleScene");
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR // ����Ƽ ������ �ʿ����� �۾�
        UnityEditor.EditorApplication.isPlaying = false;
        //������ �ٷ� ������ ���(�����, �����)
#else
        Application.Quit(); // ���� ��Ȱ��ȭ�Ǵ� �ڵ尡 �ٷ� ����
#endif
    }

}