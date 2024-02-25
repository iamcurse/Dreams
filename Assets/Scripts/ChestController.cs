using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [ShowOnly][SerializeField] private bool isOpen;
    [SerializeField] private AudioClip audioClip;
    private Animator _animator;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private UnityEvent chestItem;

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
}
