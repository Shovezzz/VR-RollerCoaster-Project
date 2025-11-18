using UnityEngine;
using UnityEngine.InputSystem; 

public class PauseController : MonoBehaviour
{
    [Tooltip("—сылка на ассет с Input Actions")]
    public InputActionAsset actionAsset;
    [Tooltip("—сылка на ваш MenuManager_Simple")]
    public MenuManager menuManager;

    private InputAction _menuAction;

    void Awake()
    {
        _menuAction = actionAsset.FindActionMap("XRI Left Interaction").FindAction("Menu");
        if (menuManager == null)
        {
            Debug.LogError("MenuManager не назначен в PauseController");
        }
    }

    void OnEnable()
    {
        _menuAction.Enable();
        _menuAction.performed += OnMenuButtonPressed;
    }

    void OnDisable()
    {
        _menuAction.Disable();
        _menuAction.performed -= OnMenuButtonPressed;
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        menuManager.TogglePauseMenu();
    }
}