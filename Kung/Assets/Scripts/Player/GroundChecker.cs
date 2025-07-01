using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Tooltip("땅으로 간주할 노멀 벡터의 Y값 최소 임계치입니다.")]
    [Range(0.5f, 1.0f)] // 0.5f는 약 60도, 1.0f는 0도를 의미
    public float _groundNormalThreshold = 0.8f; // 땅으로 간주할 노멀 벡터의 Y값 (0.7f ~ 1.0f 사이 권장)

    private bool _isGrounded; // 현재 땅에 닿아있는지 여부

    // 땅에 닿았는지 확인하는 프로퍼티 (다른 스크립트에서 접근 가능)
    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    void Start()
    {
        // 초기화
        _isGrounded = false;
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForGround(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckForGround(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌이 끝났을 때, 땅에서 떨어졌다고 가정할 수 있습니다.
        // 하지만 만약 여러 개의 ContactPoint 중 하나만 떨어져도 문제가 될 수 있으니
        // IsGrounded를 true로 유지하는 다른 로직이 필요할 수 있습니다.
        // 여기서는 간단하게 모든 충돌이 끝나면 false로 설정합니다.
        // 보다 정확한 로직을 위해서는 OnCollisionStay에서 지속적으로 체크하는 것이 좋습니다.
        _isGrounded = false;
    }

    void CheckForGround(Collision2D collision)
    {
        // 각 충돌 지점(ContactPoint)을 순회합니다.
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 충돌 지점의 노멀 벡터를 가져옵니다.
            Vector3 normal = contact.normal;

            // 노멀 벡터의 Y값이 groundNormalThreshold보다 크거나 같으면 땅으로 간주합니다.
            // Vector3.Dot(normal, Vector3.up)은 normal 벡터와 위쪽 방향 벡터(0,1,0)의 내적을 계산합니다.
            // 두 벡터가 같은 방향을 가리킬수록 1에 가까워지고, 수직일수록 0에 가까워집니다.
            if (Vector3.Dot(normal, Vector3.up) >= _groundNormalThreshold)
            {
                //Debug.Log("닿음");
                _isGrounded = true;
                return; // 땅을 찾았으므로 더 이상 확인할 필요가 없습니다.
            }
        }
        // 모든 ContactPoint를 확인했음에도 땅을 찾지 못했다면
        _isGrounded = false;
    }

    // 디버그용: 에디터에서 플레이할 때 땅에 닿았는지 시각적으로 확인할 수 있습니다.
    void OnGUI()
    {
        GUI.color = _isGrounded ? Color.green : Color.red;
        GUI.Label(new Rect(10, 10, 200, 20), "Is Grounded: " + _isGrounded);
    }
}