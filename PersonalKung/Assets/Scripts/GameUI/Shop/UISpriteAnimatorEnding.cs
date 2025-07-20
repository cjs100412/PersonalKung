using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UISpriteAnimatorEnding : MonoBehaviour
{
    public Image uiImage;
    public Sprite[] frames;
    public float frameRate = 0.1f;

    private int currentIndex = 0;
    private Coroutine animCoroutine;

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        while (true)
        {
            if (currentIndex == frames.Length - 1)
            {
                uiImage.sprite = frames[currentIndex];
                Stop();
            }
            uiImage.sprite = frames[currentIndex];
            currentIndex = (currentIndex + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }

    public void Stop()
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        CallTitleScene();
    }

    public void CallTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
