using UnityEngine;
using UnityEngine.UI;

public class UIButtonAlpha : MonoBehaviour
{
    private Image buttonImage;

    private void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();
    }

    public void SetAlpha(float a)
    {
        Color c = buttonImage.color;
        c.a = a;
        buttonImage.color = c;
    }

    public void OnClickButton()
    {
        SetAlpha(1);
    }
}
