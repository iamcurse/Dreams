using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isOpen;
    [SerializeField] private bool openByDefault;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    private Animator _animator;
    private BoxCollider2D[] _boxCollider2Ds;
    private GameObject _closeDoorFrame;
    private GameObject _openDoorFrame;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    [SerializeField] private bool smallDoor;

    private InventoryManager _inventoryManager;
    [SerializeField] private Item key;
    
    private InteractableObject _interactableObject;
    
    private void Awake()
    {
        if (!smallDoor)
        {
            _closeDoorFrame = transform.GetChild(0).GameObject();
            _openDoorFrame = transform.GetChild(1).GameObject();
        }

        _inventoryManager = FindFirstObjectByType<InventoryManager>();
        _interactableObject = GetComponent<InteractableObject>();
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2Ds = GetComponents<BoxCollider2D>();
        if (openByDefault)
            OpenDoorNoSound();
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("CheckKey", this, SymbolExtensions.GetMethodInfo(() => CheckKey()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("CheckKey");
    }

    private bool CheckKey()
    {
        return _inventoryManager.CheckItem(key);
    }
    
    public void DoorInteract()
    {
        OpenDoor();
        _inventoryManager.Remove(key);
        var a = DialogueLua.GetItemField("Key", "Amount").asInt;
        DialogueLua.SetItemField("Key", "Amount", a - 1);
        _interactableObject.disable = true;
    }

    private void OpenDoor()
    {
        if (isOpen) return;
        
        isOpen = true;
        if (doorOpen)
            AudioSource.PlayClipAtPoint(doorOpen, transform.position);
        
        DoorPropertiesChange();
    }

    // private void CloseDoor()
    // {        
    //     if (!isOpen) return;
    //     
    //     isOpen = false;
    //     if (doorOpen)
    //         AudioSource.PlayClipAtPoint(doorClose, transform.position);
    //     
    //     DoorPropertiesChange();
    // }
    
    private void OpenDoorNoSound()
    {
        if (isOpen) return;
        isOpen = true;
        DoorPropertiesChange();
    }
    
    // private void CloseDoorNoSound()
    // {
    //     if (!isOpen) return;
    //     isOpen = false;
    //     DoorPropertiesChange();
    // }

    private void DoorPropertiesChange()
    {
        switch (isOpen)
        {
            case true:
                _animator.SetBool(IsOpen, isOpen);
                _boxCollider2Ds[0].enabled = false;
                if (!smallDoor)
                {
                    _closeDoorFrame.SetActive(false);
                    _openDoorFrame.SetActive(true);
                }
                break;
            default:
                _animator.SetBool(IsOpen, isOpen);
                _boxCollider2Ds[0].enabled = true;
                if (!smallDoor)
                {
                    _closeDoorFrame.SetActive(true);
                    _openDoorFrame.SetActive(false);
                }
                break;
        }
    }
}
