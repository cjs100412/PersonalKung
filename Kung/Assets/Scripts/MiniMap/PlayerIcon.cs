using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float blinkSpeed = 3.5f;

    private void Start()
    {
    }

    private void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        Color spriteColor = _sprite.color;
        spriteColor.a = alpha;
        _sprite.color = spriteColor;
    }
}
