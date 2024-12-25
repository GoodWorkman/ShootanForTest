using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput _playerInput;
    private InputSystemUIInputModule _UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        _UIInputModule = GetComponent<InputSystemUIInputModule>();
        _UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null); 
        _UIInputModule.enabled = true; 
    }
   
    public void DisableAllUIInputs()
    {
        _playerInput.DisableAllInputs(); 
        _UIInputModule.enabled = false; 
    }
}