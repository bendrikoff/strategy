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
        InitializeControls();
    }
    public void Enter()
    {
        _cameraController.EnterIdleState();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    private void OnClick()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<Building>(out var building))
        {
            UIEvents.OnSelectedBuilding?.Invoke(building);
        }
    }
    
    private void InitializeControls()
    {
        _controls.Player.Click.performed += ctx => OnClick();
    }
}
