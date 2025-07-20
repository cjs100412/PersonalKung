using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _positionControl;  // ������ ���� ������ ����

    void Start()
    {
        // ���� ī�޶� ��ġ���� Ÿ�ٱ����� �������� ����
       _positionControl = transform.position - _player.position;
    }

    void LateUpdate()
    {
        Vector3 currentPos = _player.position + _positionControl;
        
        transform.position = currentPos;
    }
}
