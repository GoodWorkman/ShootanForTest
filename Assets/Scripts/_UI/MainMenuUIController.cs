using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Canvas")] 
    [SerializeField] private Canvas _mainMenuCanvas;

    [Header("Buttons")] 
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonQuit;

    private void OnEnable()
    {
        _buttonStart.onClick.AddListener(OnStartGameBUttonClick);
        _buttonQuit.onClick.AddListener(OnButtonQuitClicked);
    }

    private void OnDisable()
    {
        _buttonStart.onClick.RemoveAllListeners();
        _buttonQuit.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(_buttonStart);
    }

    void OnStartGameBUttonClick()
    {
        _mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadMainScene();
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}