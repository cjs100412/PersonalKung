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
        op.allowSceneActivation = false;  // 0.9���� �ε� �� ���

        while(op.progress < 0.9f)
        {
            yield return null;  // ���� �����ӱ��� ���
        }
        op.allowSceneActivation = true;
    }
}
