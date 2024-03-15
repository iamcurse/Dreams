using UnityEngine;

public class DialogueInteractHandler : MonoBehaviour
{
    private PlayerControl _playerControl;

    private void Awake()
    {
        _playerControl = FindFirstObjectByType<PlayerControl>();
    }
    
    public void TurnOffDialogue()
    {
        _playerControl.dialogueOpen = false;
        _playerControl.uiController.ShowControlUI();
    }
    public void TurnOnDialogue()
    {
        _playerControl.dialogueOpen = true;
       _playerControl.uiController.HideControlUI();
    }
    
    public void TurnOffMenu()
    {
        _playerControl.menuOpen = false;
        _playerControl.uiController.ShowControlUI();
    }
    public void TurnOnMenu()
    {
        _playerControl.menuOpen = true;
        _playerControl.uiController.HideControlUI();
    }
}
