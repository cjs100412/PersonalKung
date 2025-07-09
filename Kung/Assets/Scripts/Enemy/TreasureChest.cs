using System.Linq;
using TMPro;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private Sprite _chestOpen;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private InventoryRepositoryLocatorSO _inventoryRepository;
    [SerializeField] private Canvas _chestDialogCanvas;
    [SerializeField] private TextMeshProUGUI _chestDialogText;
    [SerializeField] private GameObject _chestObject;
    private PlayerHealth _playerHealth;
    private ShortcutKey _shortcutKey;
    private int randomNumber;

    private void Awake()
    {
        _shortcutKey = GameObject.Find("ShortcutKey").GetComponent<ShortcutKey>();
        _playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _chestDialogCanvas.gameObject.SetActive(true);
            Debug.Log("Treasure Chest Open");
            spriteRenderer.sprite = _chestOpen;
            randomNumber = Random.Range(1001, 1007);
            if(randomNumber == 1006)
            {
                _playerHealth.gold = _playerHealth.gold.AddGold(500);
                _chestDialogText.text = "500G";
            }
            else
            {
                _shortCutServiceLocator.Service.AddShortCutItemQuantity(randomNumber);
                _chestDialogText.text = _inventoryRepository.Repository.FindById(randomNumber).DevName.ToString();
            }
            _shortcutKey.Refresh();
            Destroy(_chestObject, 1f);
        }
    }
}
