using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [SerializeField] private bool openByDefault;
    [ShowOnly][SerializeField] private bool isOpen;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    private Animator _animator;
    private BoxCollider2D[] _boxCollider2Ds;
    private GameObject _closeDoorFrame;
    private GameObject _openDoorFrame;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    [SerializeField] private bool smallDoor;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    public void DoorInteract()
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

    private void OpenDoor()
    {
        if (!isInRange) return;
        if (isOpen) return;
        
        isOpen = true;
        if (doorOpen)
            AudioSource.PlayClipAtPoint(doorOpen, transform.position);
        
        DoorPropertiesChange();
    }

    private void CloseDoor()
    {        
        if (!isInRange) return;
        if (!isOpen) return;
        
        isOpen = false;
        if (doorOpen)
            AudioSource.PlayClipAtPoint(doorClose, transform.position);
        
        DoorPropertiesChange();
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
