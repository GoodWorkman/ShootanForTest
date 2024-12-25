using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Canvas _hUDCanvas;
    [SerializeField] private AudioData _confirmGameOverSound;

    private Canvas canvas;
    private Animator animator;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
        _input.onGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
        _input.onGameOver -= OnConfirmGameOver;
    }

    private void OnGameOver()
    {
        _hUDCanvas.enabled = false; 
        canvas.enabled = true;
        animator.enabled = true;
        _input.DisableAllInputs(); 
    }

    void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySfx(_confirmGameOverSound);
        _input.DisableAllInputs();
        animator.Play("GameOverScreenExit");
        SceneLoader.Instance.LoadMenuScene();
    }
    
    void EnableGameOverInput()
    {
        _input.EnableGameOverInput();
    }
}