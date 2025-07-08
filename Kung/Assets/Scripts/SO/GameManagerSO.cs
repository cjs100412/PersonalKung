using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "Scriptable Objects/GameManagerSO")]
public class GameManagerSO : ScriptableObject
{
    public GameManager GameManager { get; private set; }
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;

    public void Bootstrap()
    {
        if (GameManager != null) return;

        var existing = FindAnyObjectByType<GameManager>();
        if (existing != null)
        {
            GameManager = existing;
            return;
        }

        var gameObjecct = new GameObject("GameManager");
        var gameManager = gameObjecct.AddComponent<GameManager>();
        gameManager.Init(_shortCutServiceLocator);

        DontDestroyOnLoad(gameObjecct);

        GameManager = gameManager;
    }
}
