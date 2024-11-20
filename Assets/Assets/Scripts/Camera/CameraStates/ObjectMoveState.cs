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

    private BuildMoveService _objectMoveService;

    public ObjectMoveState(
        Camera camera, 
        PlayerControls controls,
        float sensitivity,
        Vector2 max,
        Vector2 min,
        BuildMoveService objectMoveService)

    {
        _camera = camera;
        _controls = controls;
        _sensitivity = sensitivity;
        _max = max;
        _min = min;
        _objectMoveService = objectMoveService;
    }

    public void Enter()
    {
        EnableControls();
    }

    public void Execute()
    {
        if (_isDragging)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();

            Vector2 delta = currentMousePosition - _lastMousePosition;

            Vector3 movement = new Vector3(-delta.x, -delta.y, 0) * _sensitivity;

            _camera.transform.Translate(movement * Time.deltaTime);

            float clampedX = Mathf.Clamp(_camera.transform.position.x, _min.x, _max.x);
            float clampedY = Mathf.Clamp(_camera.transform.position.y, _min.y, _max.y);

            _camera.transform.position = new Vector3(clampedX, clampedY, _camera.transform.position.z);

            _lastMousePosition = currentMousePosition;
        }
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
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
