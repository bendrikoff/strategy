using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using Assets.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayingPlayState : IState
{
    private CameraController _cameraController;
    private readonly PlayerControls _controls;

    public PlayingPlayState(CameraController cameraController)
    {
        _cameraController = cameraController;
        _controls = new PlayerControls();
    }
    public void Enter()
    {
        _controls.Enable();
        SubscribeControls();
        _cameraController.EnterIdleState();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _controls.Disable();
        UnSubscribeControls();
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        var hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<Building>(out var building))
        {
            UIEvents.OnSelectedBuilding?.Invoke(building);
        }
    }
    
    private void SubscribeControls()
    {
        _controls.Player.Click.performed += OnClick;
    }
    
    private void UnSubscribeControls()
    {
        _controls.Player.Click.performed += OnClick;
    }
}
