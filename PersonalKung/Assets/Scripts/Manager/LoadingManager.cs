using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("KungGameScene");
        op.allowSceneActivation = false;  // 0.9까지 로딩 후 대기

        while(op.progress < 0.9f)
        {
            yield return null;  // 다음 프레임까지 대기
        }
        op.allowSceneActivation = true;
    }
}
