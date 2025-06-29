using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform player;
    private Vector3 positionControl; 

    public bool followX = false;
    public bool followY = true;

    void Start()
    {
        if (player != null)
        {
            positionControl = transform.position - player.position;   // 카메라 위치랑 플레이어 위치 조절
        }
    }

    // LateUpdate로 튕김 방지
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 currentPos = transform.position;

            if (followX) 
            { 
                currentPos.x = player.position.x + positionControl.x;
            }

            if (followY)
            {
                currentPos.y = player.position.y + positionControl.y;
            }

            currentPos.z = player.position.z + positionControl.z;  // z좌표는 유지
            transform.position = currentPos;
        }
    }
}
