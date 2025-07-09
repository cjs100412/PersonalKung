using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Tooltip("땅으로 인식할 표면의 최소 Y축 법선 벡터값입니다. (1에 가까울수록 평평한 면만 땅으로 인식)")]
    [Range(0.5f, 1.0f)]
    public float _groundNormalThreshold = 0.8f;

    private bool _isGrounded;

    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    void FixedUpdate()
    {
        _isGrounded = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckForGround(collision);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForGround(collision);
    }

    void CheckForGround(Collision2D collision)
    {
        if (_isGrounded) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) >= _groundNormalThreshold)
            {
                _isGrounded = true;
                return; 
            }
        }
    }

    void OnGUI()
    {
        GUI.color = _isGrounded ? Color.green : Color.red;
        GUI.Label(new Rect(10, 10, 200, 20), "Is Grounded: " + IsGrounded);
    }
}
