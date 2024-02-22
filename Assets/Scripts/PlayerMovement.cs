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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody2D.velocity = _movementInput * moveSpeed;
        switch (_movementInput.x)
        {
            case > 0:
                _spriteRenderer.flipX = false;
                _boxCollider2D.offset = new Vector2(0.005f, -0.13f);
                break;
            case < 0:
                _spriteRenderer.flipX = true;
                _boxCollider2D.offset = new Vector2(0.005f * -1, -0.13f);
                break;
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
