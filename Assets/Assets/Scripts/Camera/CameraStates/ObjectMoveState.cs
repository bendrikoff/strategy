using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectMoveState : IState
{
    private Camera _camera;
    private PlayerControls _controls;
    
    private bool _isDragging = false;
    private Vector2 _lastMousePosition;
    private float _sensitivity;
    
    private Vector2 _max;
    private Vector2 _min;
    
    public ObjectMoveState(
        Camera camera, 
        PlayerControls controls,
        float sensitivity,
        Vector2 max,
        Vector2 min)

    {
        _camera = camera;
        _controls = controls;
        _sensitivity = sensitivity;
        _max = max;
        _min = min;
    }

    public void Enter()
    {
        EnableControls();
    }

    public void Execute()
    {
        return;
        //todo: После реализации перемещения отдельных зданий добавить перемещение камеры при перемещении здания
    }

    public void Exit()
    {
        _controls.Disable();
    }

    private void EnableControls()
    {
        _controls.Enable();
        _controls.Player.MouseDrag.performed += OnMouseDrag;
        _controls.Player.MouseDrag.canceled += OnMouseDragCanceled;
    }
    
    private void OnMouseDrag(InputAction.CallbackContext context)
    {
        _isDragging = true;
        _lastMousePosition = Mouse.current.position.ReadValue();  // Сохраняем начальную позицию мыши
    }
    
    private void OnMouseDragCanceled(InputAction.CallbackContext context)
    {
        _isDragging = false;
    }
}
