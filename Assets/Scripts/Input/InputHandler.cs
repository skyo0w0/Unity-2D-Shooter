using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    #region Input Events
    #region Movement Events
    public event Action OnRight;
    public event Action OnLeft;
    public event Action OnHorizontalReleased;
    public event Action OnUp;
    public event Action OnDown;
    public event Action OnVerticalReleased;
    #endregion
    
    #region Mouse Events
    public event Action<Vector2> OnMouseMove;
    public event Action OnFirePressed;
    #endregion
    #endregion
    
    #region Input Attributes
    private PlayerControls _playerControls;
    private InputAction _horizontalMovement;
    private InputAction _verticalMovement;
    private InputAction _fire;
    private InputAction _mousePos;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _horizontalMovement = _playerControls.Player.HorizontalMovement;
        _verticalMovement = _playerControls.Player.VerticalMovement;
        _mousePos = _playerControls.Player.MousePosition;
        _fire = _playerControls.Player.FireButton;
    }

    private void OnEnable()
    {
        _horizontalMovement.performed += HandleHorizontalMovement;
        _horizontalMovement.canceled += HandleHorizontalMovement;
        _verticalMovement.performed += HandleVerticalMovement;
        _verticalMovement.canceled += HandleVerticalMovement;
        _mousePos.performed += HandleMousePos;
        _mousePos.canceled += HandleMousePos;
        _fire.performed += HandleFirePressed;
        
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _horizontalMovement.performed -= HandleHorizontalMovement;
        _horizontalMovement.canceled -= HandleHorizontalMovement;
        _verticalMovement.performed -= HandleVerticalMovement;
        _verticalMovement.canceled -= HandleVerticalMovement;
        _mousePos.performed -= HandleMousePos;
        _mousePos.canceled -= HandleMousePos;
        _fire.performed -= HandleFirePressed;
        
        _playerControls.Disable();
    }
    #endregion
    
    #region Handle Methods
    private void HandleHorizontalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        if (direction > 0)
        {
            OnRight?.Invoke();
        }
        else if (direction < 0)
        {
            OnLeft?.Invoke();
        }
        else
        {
            OnHorizontalReleased?.Invoke();
        }
    }

    private void HandleVerticalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        if (direction > 0)
        {
            OnUp?.Invoke();
        }
        else if (direction < 0)
        {
            OnDown?.Invoke();
        }
        else
        {
            OnVerticalReleased?.Invoke();
        }
    }

    private void HandleMousePos(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        OnMouseMove?.Invoke(mousePosition);
    }
    
    private void HandleFirePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFirePressed?.Invoke();
        }
    }
    #endregion
}

