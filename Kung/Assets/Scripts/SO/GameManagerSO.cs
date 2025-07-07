using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "Scriptable Objects/GameManagerSO")]
public class GameManagerSO : ScriptableObject
{
    public GameManager GameManager { get; private set; }
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;

    public void Bootstrap()
    {
        //GameManager = new GameManager(_shortCutServiceLocator);
    }
}
