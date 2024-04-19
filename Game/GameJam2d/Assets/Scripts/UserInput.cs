using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UserInputActions;

/// <summary>
/// Responsible for handling all user inputs outside of UI interaction.
/// </summary>
public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }

    // list of all Bindings - if you add a new one to <c>UserInput</c> you need to add it here
    public enum Binding {
        Pause,
        MoveUp,
        MoveLeft,
        MoveDown,
        MoveRight,
        Interact,      
        
    }

    public event EventHandler OnInteractAction;      

    private UserInputActions userInputActions;

    // used to replace names for buttons that would be too long/weird to display
    Dictionary<string, string> buttonLabelMappings = new Dictionary<string, string>();

    private void Awake() {
        if (UserInput.Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Too many GameManager Instances: " + UserInput.Instance);
        }
        userInputActions = new UserInputActions();
        userInputActions.PlayerInput.Enable();

        userInputActions.PlayerInput.Pause.performed += OnPausePressed;
        userInputActions.PlayerInput.Interact.performed += OnInteractPressed;       

        LoadAllOverrides();

        // current name overrides
        buttonLabelMappings.Add("Left Arrow", "Åa");
        buttonLabelMappings.Add("Right Arrow", "Åd");
        buttonLabelMappings.Add("Up Arrow", "Åw");
        buttonLabelMappings.Add("Down Arrow", "s");
        buttonLabelMappings.Add("Escape", "esc");
    }

    

    private void OnDestroy() {
        userInputActions.PlayerInput.Disable();
    }

    
    private void OnPausePressed(InputAction.CallbackContext obj) {
       //Do something to pause/unpause the game        
    }

    private void OnInteractPressed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovevementVectorNormalized() {
        
        Vector2 direction = userInputActions.PlayerInput.WASD.ReadValue<Vector2>();        
        return direction.normalized;
    }

    

    /// <summary>
    /// Returns a displayable string for a key/action that is assigned to a binding
    /// When adding a new Binding that can be rebinded, add it here.
    /// </summary>
    /// <param name="binding"></param>
    /// <returns></returns>
    public string GetBindingText(Binding binding) {
        string result;
        switch (binding) {
            default:
            case Binding.MoveUp:
                result = userInputActions.PlayerInput.WASD.bindings[1].ToDisplayString();
                break;
            case Binding.MoveDown:
                result = userInputActions.PlayerInput.WASD.bindings[2].ToDisplayString();
                break;
            case Binding.MoveLeft:
                result = userInputActions.PlayerInput.WASD.bindings[3].ToDisplayString();
                break;
            case Binding.MoveRight:
                result = userInputActions.PlayerInput.WASD.bindings[4].ToDisplayString();
                break;
            case Binding.Pause:
                result = userInputActions.PlayerInput.Pause.bindings[0].ToDisplayString();
                break;
            case Binding.Interact:
                result = userInputActions.PlayerInput.Interact.bindings[0].ToDisplayString();
                break;           
        }
        //Debug.Log(result);
        if (result.Length > 3 && buttonLabelMappings.ContainsKey(result)) {
            return buttonLabelMappings[result];                
        }
        return result;        
    }

    /// <summary>
    /// Perforns a rebind of a given Binding.
    /// When adding a new Binding that can be rebinded, add it here.
    /// </summary>
    /// <param name="binding"></param>
    /// <param name="onActionRebound"></param>
    public void RebindBinding(Binding binding, Action onActionRebound) {
        userInputActions.PlayerInput.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch (binding) {
            default:
            case Binding.Pause:
                inputAction = userInputActions.PlayerInput.Pause;
                bindingIndex = 0;
                break;
            case Binding.Interact:
                inputAction = userInputActions.PlayerInput.Interact;
                bindingIndex = 0;
                break;           
            case Binding.MoveUp:
                inputAction = userInputActions.PlayerInput.WASD;
                bindingIndex = 1;
                break;
            case Binding.MoveLeft:
                inputAction = userInputActions.PlayerInput.WASD;
                bindingIndex = 3;
                break;
            case Binding.MoveDown:
                inputAction = userInputActions.PlayerInput.WASD;
                bindingIndex = 2;
                break;
            case Binding.MoveRight:
                inputAction = userInputActions.PlayerInput.WASD;
                bindingIndex = 4;
                break;            
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                SaveBindingOverride(inputAction);
                callback.Dispose();
                userInputActions.PlayerInput.Enable();
                onActionRebound();
            })
            .Start();
    }


    /// <summary>
    /// Loads all binding overrides. When adding a new Binding that can be rebinded, add it here.
    /// </summary>
    private void LoadAllOverrides() {
        
        LoadBindingOverride(userInputActions.PlayerInput.Pause);
        LoadBindingOverride(userInputActions.PlayerInput.WASD);
        LoadBindingOverride(userInputActions.PlayerInput.Interact);
    }

    /// <summary>
    /// Resets all binding overrides. When adding a new Binding that can be rebinded, add it here.
    /// </summary>
    public void ResetAllBindings() {
        ResetBinding(userInputActions.PlayerInput.Pause, 0);
        ResetBinding(userInputActions.PlayerInput.WASD);
        ResetBinding(userInputActions.PlayerInput.Interact, 0);        
    }

    /// <summary>
    /// The methods in this region are internal and  should not require any changes
    /// </summary>
    #region Internal methods    
    private static void SaveBindingOverride(InputAction action) {
        for(int i = 0; i < action.bindings.Count(); i++) {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
            //Debug.Log("Saved binding override for " + action.actionMap + action.name + i + " as " + action.bindings[i].overridePath);
        }
        
    }
    public  void LoadBindingOverride(InputAction action) {
        if (userInputActions == null)
            userInputActions = new UserInputActions();               

        for (int i = 0; i < action.bindings.Count; i++) {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i))) {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
                //Debug.Log("Apllied  binding override for " + action.actionMap + action.name + i + " as " + action.bindings[i].overridePath);
            }                
        }
    }
    public void ResetBinding(InputAction action) {
        for (int i = 0; i < action.bindings.Count; i++) {
            ResetBinding(action, i);
        }
    }

    public void ResetBinding(InputAction action, int bindingIndex) {

        if (action == null || action.bindings.Count <= bindingIndex) {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite) {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                action.RemoveBindingOverride(i);
        }
        else
            action.RemoveBindingOverride(bindingIndex);

        SaveBindingOverride(action);
    }
    #endregion

}
