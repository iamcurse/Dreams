using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [ShowOnly][SerializeField] private bool isInRange;
    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    
    private Vector2 _movementInput;
    private PlayerController _playerController;
    private InputAction _move;
    
    private InputAction _interact;
    private InteractableObject _interactableObject;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    [SerializeField] private GameObject mobileUI;

    #region WebGL Mobile Check

    [DllImport("__Internal")]
    // ReSharper disable once UnusedMember.Local
    private static extern bool IsMobile();
    
    // ReSharper disable once InconsistentNaming
    private bool isMobile()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
        #endif
        return false;
    }
    
    #endregion

    private void Awake()
    {
        _playerController = new PlayerController();
        _move = _playerController.Player.Move;
        _interact = _playerController.Player.Interact;
    }
    
    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        Screen.fullScreen = !Screen.fullScreen;
    }
    
    private void FixedUpdate()
    {
        Move();
        Animate();
    }

    private void OnEnable()
    {
        if (isMobile())
            mobileUI.SetActive(true);
        _move.Enable();
        _interact.Enable();
        _interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        if (isMobile())
            mobileUI.SetActive(false);
        _move.Disable();
        _interact.Disable();
        _interact.performed -= OnInteract;
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
        _interactableObject = other.GetComponent<InteractableObject>();
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInRange = false;
    }

    private void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (isInRange)
            _interactableObject.Interact();
    }
}
