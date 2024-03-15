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
        
        var vowel = item.itemName.Substring(0, 1).ToUpper();
        if (vowel is "A" or "E" or "I" or "O" or "U") {
            DialogueLua.SetVariable("ItemNameDialogue", "an " + item.itemName);
        } else {
            DialogueLua.SetVariable("ItemNameDialogue", "a " + item.itemName);
        }
        
        var a = DialogueLua.GetItemField(item.itemName, "Amount").asInt;
        DialogueLua.SetItemField(item.itemName, "Amount", a + 1);
        chestItem.Invoke();
        
        isOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    public void AddItem()
    {
        _inventoryManager.Add(item);
    }
}
