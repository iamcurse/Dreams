using UnityEngine;

public class Trap : MonoBehaviour
{
    [ShowOnly] [SerializeField] private bool isInRange;
    [ShowOnly] public bool isActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    private void FixedUpdate()
    {
        if (!isInRange || !isActive) return;
        var uiController = FindObjectOfType<UIController>();
        uiController.GameOver();
    }
}
