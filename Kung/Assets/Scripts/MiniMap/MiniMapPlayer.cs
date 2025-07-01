using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;           

    void LateUpdate()
    {
        Vector3 currentPos = _player.position;
        transform.position = new Vector3(currentPos.x, currentPos.y, transform.position.z);
    }
}