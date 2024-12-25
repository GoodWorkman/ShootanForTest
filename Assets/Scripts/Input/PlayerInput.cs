using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Input")]

public class PlayerInput : 
    ScriptableObject,  
    InputActions.IGamePlayActions,  
    InputActions.IPauseMenuActions, 
    InputActions.IGameOverScreenActions
{

    #region Actions
        
        InputActions inputActions;
        public event UnityAction<Vector2> onMove = delegate {};  
        public event UnityAction onStopMove = delegate {};
        public event UnityAction onFire = delegate {};
        public event UnityAction onStopFire = delegate {};
        public event UnityAction onDodge = delegate {};
        public event UnityAction onOverDrive = delegate {};
        public event UnityAction onPause = delegate {};
        public event UnityAction onUnpause = delegate {};
        public event UnityAction onLaunchMissle = delegate {};
        public event UnityAction onGameOver = delegate {};

    #endregion
    
    #region GameRunning
        
        private void OnEnable() {
            inputActions = new InputActions();
            inputActions.GamePlay.AddCallbacks(this);  
            inputActions.PauseMenu.AddCallbacks(this);  
            inputActions.GameOverScreen.AddCallbacks(this);
        }

        private void OnDisable() {
            DisableAllInputs();
        }

    #endregion

    #region ActionMap
        
        private void SwitchActionMap(InputActionMap actionMap, bool isUIInput) {
            inputActions.Disable();  
            actionMap.Enable();  

            if (isUIInput) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
        public void Switch2DynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        public void Switch2FixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

        public void DisableAllInputs() => inputActions.Disable();
        public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, false); 
        public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);
        public void EnableGameOverInput() => SwitchActionMap(inputActions.GameOverScreen, true);
        

    #endregion

    #region Delegate function
        
        public void OnMove(InputAction.CallbackContext context) {
            
            if (context.phase == InputActionPhase.Performed) {
                onMove.Invoke(context.ReadValue<Vector2>());
            }
            if (context.phase == InputActionPhase.Canceled) {
                onStopMove.Invoke();
            }
        }

        public void OnFire(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) {
                onFire.Invoke();
            }
            if (context.phase == InputActionPhase.Canceled) {
                onStopFire.Invoke();
            }
        }

        public void OnDodge(InputAction.CallbackContext context) {
            if (context.performed) {
                onDodge.Invoke();
            }
        }

        public void OnOverDrive(InputAction.CallbackContext context) {
            if (context.performed) {
                onOverDrive.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context) {
            if (context.performed) {
                onPause.Invoke();
            }
        }

        public void OnUnpause(InputAction.CallbackContext context) {
            if (context.performed) {
                onUnpause.Invoke();
            }
        }

        public void OnLaunchMissle(InputAction.CallbackContext context) {
            if (context.performed) {
                onLaunchMissle.Invoke();
            }
        }

        public void OnGameOver(InputAction.CallbackContext context) {
            if (context.performed) {
                onGameOver.Invoke();
            }
        }

    #endregion
    
}
