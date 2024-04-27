using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [SerializeField] private bool decoy;
    [ShowOnly][SerializeField] private bool isOpen;
    [SerializeField] private Item item;
    [SerializeField] private AudioClip audioClip;
    private Animator _animator;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private UnityEvent dialogue;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("OpenChest", this, SymbolExtensions.GetMethodInfo(() => OpenChest("")));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("OpenChest");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isInRange = true;
    }
    private void OnTriggerExit2D(Collider2D other) => isInRange = false;

    public void Interact()
    {
        if (!isInRange) return;
        if (isOpen) return;

        DialogueLua.SetVariable("GameObjectName", name);
        if (!decoy)
        {
            DialogueLua.SetVariable("ItemName", item.itemName);
            DialogueLua.SetVariable("ItemID", item.id);
            
            var vowel = item.itemName[..1].ToUpper();
            DialogueLua.SetVariable("ItemArticle", vowel is "A" or "E" or "I" or "O" or "U" ? "an" : "a");
        }
        
        dialogue.Invoke();
    }

    private void OpenChest()
    {
        if (!isInRange) return;
        if (isOpen) return;
        _animator.SetTrigger(IsOpen);
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        isOpen = true;
    }

    private static void OpenChest(string objectName)
    {
        var chestObject = SequencerTools.FindSpecifier(objectName).GetComponent<ChestController>();
        chestObject.OpenChest();
    }
}
