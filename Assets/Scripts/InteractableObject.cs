using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [SerializeField] private UnityEvent interaction;
    [SerializeField] private UnityEvent interactionResult;
    [HideInInspector] public bool disable;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isInRange = true;
    }
    private void OnTriggerExit2D(Collider2D other) => isInRange = false;

    public void Interact()
    {
        if (!isInRange) return;
        if (disable) return;
        
        interaction.Invoke();
    }

    public void InteractResult() => interactionResult.Invoke();
    
}