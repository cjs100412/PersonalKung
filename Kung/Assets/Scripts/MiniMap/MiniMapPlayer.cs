using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    public Transform player;           

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = player.position;
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }
}
