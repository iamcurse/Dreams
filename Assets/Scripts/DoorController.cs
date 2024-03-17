using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [ShowOnly] [SerializeField] private bool isInRange;
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
    
    [SerializeField] private Item keyItem;
    [SerializeField] private UnityEvent doorInteract;
    
    private void Awake()
    {
        if (smallDoor) return;
        _closeDoorFrame = transform.GetChild(0).GameObject();
        _openDoorFrame = transform.GetChild(1).GameObject();
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
        Lua.RegisterFunction("DoorControl", this, SymbolExtensions.GetMethodInfo(() => DoorControl("")));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("DoorControl");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    public void Interact()
    {
        if (isOpen) return;
        if (!isInRange) return;
        
        DialogueLua.SetVariable("GameObjectName", name);
        DialogueLua.SetVariable("ItemName", keyItem.itemName);
        DialogueLua.SetVariable("ItemID", keyItem.id);
        
        doorInteract.Invoke();
    }

    private void OpenDoor()
    {
        if (isOpen) return;
        
        isOpen = true;
        if (doorOpen)
            AudioSource.PlayClipAtPoint(doorOpen, transform.position);
        
        DoorPropertiesChange();
    }

    private void CloseDoor()
    {        
        if (!isOpen) return;
        
        isOpen = false;
        if (doorOpen)
            AudioSource.PlayClipAtPoint(doorClose, transform.position);
        
        DoorPropertiesChange();
    }

    private void DoorControl()
    {
        switch (isOpen)
        {
            case true:
                CloseDoor();
                break;
            default:
                OpenDoor();
                break;
        }
    }
    private static void DoorControl(string objectName)
    {
        var doorObject = SequencerTools.FindSpecifier(objectName).GetComponent<DoorController>();
        doorObject.DoorControl();
    }
    
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
