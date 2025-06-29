using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float blinkSpeed = 3.5f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (sprite != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            Color spriteColor = sprite.color;
            spriteColor.a = alpha;
            sprite.color = spriteColor;
        }
    }
}
