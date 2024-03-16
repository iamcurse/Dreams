using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [ShowOnly][SerializeField] private bool isInRange;
    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    
    private Vector2 _movementInput;
    private PlayerController _playerController;
    private InputAction _move;
    [HideInInspector] public bool lockMovement;
    
    private InputAction _interact;
    private InteractableObject _interactableObject;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    [HideInInspector] public UIController uiController;

    [HideInInspector] public InventoryManager inventoryManager;

    public bool dialogueOpen;
    public bool menuOpen;

    private void Awake()
    {
        _playerController = new PlayerController();
        uiController = FindFirstObjectByType<UIController>();
        _move = _playerController.Player.Move;
        _interact = _playerController.Player.Interact;

        inventoryManager = FindFirstObjectByType<InventoryManager>();
    }
    
    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        uiController.ShowControlUI();
        _move.Enable();
        _interact.Enable();
        
        Lua.RegisterFunction("InteractResult", this, SymbolExtensions.GetMethodInfo(() => InteractResult()));
    }

    private void OnDisable()
    {
        if (uiController)
            uiController.HideControlUI();
        _move.Disable();
        _interact.Disable();
        
        Lua.UnregisterFunction("InteractResult");
    }

    private void FixedUpdate()
    {
        Move();
        Animate();
    }

    private void Move()
    {
        _movementInput = _move.ReadValue<Vector2>();
        _rigidBody2D.velocity = _movementInput * moveSpeed;
    }

    private void Animate()
    {
        _animator.SetFloat(MoveX, _movementInput.x);
        _animator.SetFloat(MoveY, _movementInput.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Objects")) return;
        isInRange = true;
        
        _interactableObject = other.GetComponent<InteractableObject>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }
    
    public void OnInteract()
    {
        if (!uiController.onMobile)
            Interact();
    }

    public void Interact()
    {
        if (!isInRange)
            return;
        if (dialogueOpen)
            return;
        if (menuOpen)
            return;
        _interactableObject.Interact();
    }

    private void InteractResult()
    {
        _interactableObject.InteractResult();
    }
}
