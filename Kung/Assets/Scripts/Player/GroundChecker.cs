using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Tooltip("������ ������ ��� ������ Y�� �ּ� �Ӱ�ġ�Դϴ�.")]
    [Range(0.5f, 1.0f)] // 0.5f�� �� 60��, 1.0f�� 0���� �ǹ�
    public float _groundNormalThreshold = 0.8f; // ������ ������ ��� ������ Y�� (0.7f ~ 1.0f ���� ����)

    private bool _isGrounded; // ���� ���� ����ִ��� ����

    // ���� ��Ҵ��� Ȯ���ϴ� ������Ƽ (�ٸ� ��ũ��Ʈ���� ���� ����)
    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    void Start()
    {
        // �ʱ�ȭ
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
        // �浹�� ������ ��, ������ �������ٰ� ������ �� �ֽ��ϴ�.
        // ������ ���� ���� ���� ContactPoint �� �ϳ��� �������� ������ �� �� ������
        // IsGrounded�� true�� �����ϴ� �ٸ� ������ �ʿ��� �� �ֽ��ϴ�.
        // ���⼭�� �����ϰ� ��� �浹�� ������ false�� �����մϴ�.
        // ���� ��Ȯ�� ������ ���ؼ��� OnCollisionStay���� ���������� üũ�ϴ� ���� �����ϴ�.
        _isGrounded = false;
    }

    void CheckForGround(Collision2D collision)
    {
        // �� �浹 ����(ContactPoint)�� ��ȸ�մϴ�.
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // �浹 ������ ��� ���͸� �����ɴϴ�.
            Vector3 normal = contact.normal;

            // ��� ������ Y���� groundNormalThreshold���� ũ�ų� ������ ������ �����մϴ�.
            // Vector3.Dot(normal, Vector3.up)�� normal ���Ϳ� ���� ���� ����(0,1,0)�� ������ ����մϴ�.
            // �� ���Ͱ� ���� ������ ����ų���� 1�� ���������, �����ϼ��� 0�� ��������ϴ�.
            if (Vector3.Dot(normal, Vector3.up) >= _groundNormalThreshold)
            {
                //Debug.Log("����");
                _isGrounded = true;
                return; // ���� ã�����Ƿ� �� �̻� Ȯ���� �ʿ䰡 �����ϴ�.
            }
        }
        // ��� ContactPoint�� Ȯ���������� ���� ã�� ���ߴٸ�
        _isGrounded = false;
    }

    // ����׿�: �����Ϳ��� �÷����� �� ���� ��Ҵ��� �ð������� Ȯ���� �� �ֽ��ϴ�.
    void OnGUI()
    {
        GUI.color = _isGrounded ? Color.green : Color.red;
        GUI.Label(new Rect(10, 10, 200, 20), "Is Grounded: " + IsGrounded);
    }
}