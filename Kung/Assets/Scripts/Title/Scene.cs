using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("KungGameScene");
    }
}
