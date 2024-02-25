using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [ShowOnly][SerializeField] private bool isInRange;
    [SerializeField] private UnityEvent interaction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    public void Interact()
    {
        if (!isInRange) return;
        
        interaction.Invoke();
    } 
}