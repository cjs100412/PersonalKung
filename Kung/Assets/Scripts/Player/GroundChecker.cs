using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform _groundCheckPoint;

    [SerializeField] private float _groundCheckRadius = 0.05f;

    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }
    private void OnDrawGizmos()
    {
        if (_groundCheckPoint != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
        }
    }

    //void OnGUI()
    //{
    //    GUI.color = _isGrounded ? Color.green : Color.red;
    //    GUI.Label(new Rect(10, 10, 200, 20), "Is Grounded: " + IsGrounded);
    //}
}
