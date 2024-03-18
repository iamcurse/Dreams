using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private bool activate;
    public bool oneTimeUse;
    private bool _used;
    [SerializeField] private UnityEvent switchInteract;
    [SerializeField] private UnityEvent switchActive;
    [SerializeField] private UnityEvent switchDeactivate;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("SwitchActivate", this, SymbolExtensions.GetMethodInfo(() => SwitchActivate("")));
        Lua.RegisterFunction("SwitchDeactivate", this, SymbolExtensions.GetMethodInfo(() => SwitchDeactivate("")));
        Lua.RegisterFunction("SwitchCurrentState", this, SymbolExtensions.GetMethodInfo(() => CurrentState("")));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SwitchActivate");
        Lua.UnregisterFunction("SwitchDeactivate");
        Lua.UnregisterFunction("SwitchCurrentState");
    }

    public void Interact()
    {
        if (oneTimeUse && _used) return;
        DialogueLua.SetVariable("GameObjectName", name);
        switchInteract.Invoke();
    }

    private void SwitchActivate()
    {
        _used = true;
        activate = true;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        switchActive.Invoke();
    }
    private void SwitchDeactivate()
    {
        _used = true;
        activate = false;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        switchDeactivate.Invoke();
    }

    private void SwitchActivate(string objectName)
    {
        var switchObject = SequencerTools.FindSpecifier(objectName).GetComponent<SwitchController>();
        switchObject.SwitchActivate();
    }
    private void SwitchDeactivate(string objectName)
    {
        var switchObject = SequencerTools.FindSpecifier(objectName).GetComponent<SwitchController>();
        switchObject.SwitchDeactivate();
    }
    
    private bool CurrentState()
    {
        return activate;
    }
    private bool CurrentState(string objectName)
    {
        var switchObject = SequencerTools.FindSpecifier(objectName).GetComponent<SwitchController>();
        return switchObject.CurrentState();
    }
}
