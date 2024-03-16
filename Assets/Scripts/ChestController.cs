using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [ShowOnly][SerializeField] private bool isOpen;
    [SerializeField] private Item item;
    [SerializeField] private AudioClip audioClip;
    private Animator _animator;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private UnityEvent chestItem;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (!isInRange) return;
        if (isOpen) return;
        
        _animator.SetTrigger(IsOpen);
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, transform.position);

        DialogueLua.SetVariable("ItemName", item.itemName);
        DialogueLua.SetVariable("ItemID", item.id);
        
        var vowel = item.itemName.Substring(0, 1).ToUpper();
        DialogueLua.SetVariable("ItemArticle", vowel is "A" or "E" or "I" or "O" or "U" ? "an" : "a");

        isOpen = true;
        
        chestItem.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) => isInRange = false;

    public void AddItem() => _inventoryManager.AddItem(item);
}
