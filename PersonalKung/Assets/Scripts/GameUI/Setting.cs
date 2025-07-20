using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [Header("Pause UI Panel")]
    [Tooltip("ESC 눌렀을 때 켜질 UI Panel")]
    public GameObject pauseCanvas;


    void Start()
    {
        // 시작할 때 반드시 꺼두기
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
        // Audio도 함께 멈추기
        //AudioListener.pause = true;
        // UI 보이기
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        // Audio도 재생
        //AudioListener.pause = false;
        // UI 숨기기
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
#if UNITY_EDITOR // 유니티 에디터 쪽에서의 작업
        UnityEditor.EditorApplication.isPlaying = false;
        //누르면 바로 꺼지는 기능(모바일, 빌드용)
#else
        Application.Quit(); // 현재 비활성화되는 코드가 바로 적용
#endif
    }

}