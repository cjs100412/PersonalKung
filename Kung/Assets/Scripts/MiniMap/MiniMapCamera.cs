using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _positionControl; 

    public bool followX = false;
    public bool followY = true;

    void Start()
    {
        _positionControl = transform.position - _player.position;   // 카메라 위치랑 플레이어 위치 조절
    }

    // LateUpdate로 튕김 방지
    void LateUpdate()
    {
        Vector3 currentPos = transform.position;

        currentPos = _player.position + _positionControl;

        //if (followX) 
        //{ 
        //    currentPos.x = _player.position.x + _positionControl.x;
        //}

        //if (followY)
        //{
        //    currentPos.y = _player.position.y + _positionControl.y;
        //}

        // currentPos.z = transform.position;  // z좌표는 유지
        transform.position = currentPos;
    }
}
