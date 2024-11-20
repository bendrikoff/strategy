using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : IState
{
    private Camera _camera;
    private PlayerControls _controls;
    
    private bool _isDragging = false;
    private Vector2 _lastMousePosition;
    private float _sensitivity;
    
    private Vector2 _max;
    private Vector2 _min;

    public IdleState(
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
        if (_isDragging)
        {
            // Получаем текущую позицию мыши
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();

            // Вычисляем изменение позиции мыши (разницу)
            Vector2 delta = currentMousePosition - _lastMousePosition;

            // Перемещаем камеру в зависимости от изменения позиции мыши
            Vector3 movement = new Vector3(-delta.x, -delta.y, 0) * _sensitivity;

            // Двигаем камеру
            _camera.transform.Translate(movement * Time.deltaTime);

            // Применяем границы для движения камеры
            float clampedX = Mathf.Clamp(_camera.transform.position.x, _min.x, _max.x);
            float clampedY = Mathf.Clamp(_camera.transform.position.y, _min.y, _max.y);

            // Обновляем позицию камеры, учитывая границы
            _camera.transform.position = new Vector3(clampedX, clampedY, _camera.transform.position.z);

            // Обновляем последнюю позицию мыши
            _lastMousePosition = currentMousePosition;
        }
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
