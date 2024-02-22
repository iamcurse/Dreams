using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D _rigidBody2D;
    private Vector2 _movementInput;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody2D.velocity = _movementInput * moveSpeed;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
