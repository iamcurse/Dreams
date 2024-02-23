using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isOpen;
    private Animator _animator;
    [SerializeField] private AudioClip audioClip;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private UnityEvent chestItem;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (isOpen) return;
        
        _animator.SetTrigger(IsOpen);
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        
        chestItem.Invoke();
        
        Debug.Log("Chest Open");
        isOpen = true;
    }
}
