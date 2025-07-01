using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _positionControl;  // 시작할 때의 오프셋 저장

    void Start()
    {
        // 현재 카메라 위치에서 타겟까지의 오프셋을 저장
       _positionControl = transform.position - _player.position;
    }

    void LateUpdate()
    {
        Vector3 currentPos = _player.position + _positionControl;
        
        transform.position = currentPos;
    }
}
