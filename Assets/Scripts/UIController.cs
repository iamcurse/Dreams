using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private bool checkMobile;
    [ShowOnly] public bool onMobile;
    [ShowOnly] public string sceneName;
    private bool _onWebGL;
    private PlayerControl _playerControl;
    private GameObject _mobileUI;
    private GameObject _fullscreenUI;
    private GameObject _gameOver;

    #region WebGL Mobile Check
    [DllImport("__Internal")]
    // ReSharper disable once UnusedMember.Local
    private static extern bool IsMobile();

    // ReSharper disable once InconsistentNaming
    private void isMobile()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            onMobile = IsMobile();
            _onWebGL = true;
        #endif
        #if !UNITY_EDITOR && UNITY_ANDROID
            onMobile = true;
        #endif
    }
    #endregion

    #region Scene Name From Index

    private static string NameFromIndex(int buildIndex)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        var slash = path.LastIndexOf('/');
        var name = path.Substring(slash + 1);
        var dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
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
        _gameOver = transform.GetChild(3).gameObject;
    }

    private void Start()
    {
        sceneName = NameFromIndex(SceneManager.GetActiveScene().buildIndex);
        if (!onMobile) return;
        if (_playerControl.gameObject.activeSelf)
            ShowControlUI();
        if (_onWebGL)
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

    public void GameOver()
    {
        _gameOver.SetActive(true);
        Time.timeScale = 0f;
        
        if (!onMobile) return;
        HideControlUI();
    }

    public void Retry()
    {
        SceneManager.LoadScene(sceneName);
        _gameOver.SetActive(false);
        Time.timeScale = 1f;
        
        if (!onMobile) return;
        ShowControlUI();
    }
}
