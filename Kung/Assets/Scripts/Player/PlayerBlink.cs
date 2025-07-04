using System.Collections;
using UnityEngine;

public class PlayerBlink : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private SpriteRenderer _damageSpriteRenderer;

    private Coroutine _blinkCoroutine;
    private bool _blinking;

    void Update()
    {
        if (_playerHealth.isDamaged && _blinkCoroutine == null)
        {
            _blinkCoroutine = StartCoroutine(DamageBlink());
        }
        else if (!_playerHealth.isDamaged && _blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
            SetPlayerAlpha(1f);
        }
    }

    private IEnumerator DamageBlink()
    {
        while (_playerHealth.isDamaged)
        {
            _blinking = !_blinking;
            SetPlayerAlpha(_blinking ? 1f : 0f);
            yield return new WaitForSeconds(0.1f); 
        }
    }

    private void SetPlayerAlpha(float alpha)
    {
        Color color = _damageSpriteRenderer.color;
        color.a = alpha;
        _damageSpriteRenderer.color = color;
    }
}
