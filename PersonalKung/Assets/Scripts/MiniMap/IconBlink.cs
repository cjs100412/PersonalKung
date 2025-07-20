using UnityEngine;
using UnityEngine.UI;

public class IconBlink : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float _blinkSpeed = 3.5f;

    private void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * _blinkSpeed));
        Color spriteColor = _sprite.color;
        spriteColor.a = alpha;
        _sprite.color = spriteColor;
    }
}
