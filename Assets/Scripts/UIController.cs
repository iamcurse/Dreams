using UnityEngine;
using System.Runtime.InteropServices;

public class UIController : MonoBehaviour
{
    [SerializeField] private bool checkMobile;
    [ShowOnly] public bool onMobile;
    private PlayerControl _playerControl;
    private GameObject _mobileUI;
    private GameObject _fullscreenUI;

    #region WebGL Mobile Check
    [DllImport("__Internal")]
    // ReSharper disable once UnusedMember.Local
    private static extern bool IsMobile();

    // ReSharper disable once InconsistentNaming
    private void isMobile()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            onMobile = IsMobile();
        #endif
        #if !UNITY_EDITOR && UNITY_ANDROID
            onMobile = true;
        #endif
    }
    #endregion
    
    private void Awake()
    {
        isMobile();
        #region Check Mobile Force Disable

        #if UNITY_EDITOR
            if (checkMobile)
                onMobile = true;
        #endif
        
        #endregion
        _playerControl = FindFirstObjectByType<PlayerControl>();
        _mobileUI = transform.GetChild(0).gameObject;
        _fullscreenUI = transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        if (!onMobile) return;
        if (_playerControl.gameObject.activeSelf)
            ShowControlUI();
        _fullscreenUI.SetActive(true);
    }

    public void ShowControlUI()
    {
        if (onMobile)
            _mobileUI.SetActive(true);
    }

    public void HideControlUI()
    {
        if (onMobile)
            _mobileUI.SetActive(false);
    }
    
    public void Fullscreen() => Screen.fullScreen = !Screen.fullScreen;

    public void MobileInteract() => _playerControl.Interact();
}
