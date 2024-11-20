using Assets.Scripts.Camera;
using Assets.Scripts.Infrastructure;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    public float sensitivity = 1f;
    
    public Vector2 Max = new Vector2(10,5);
    public Vector2 Min = new Vector2(-10, -5);

    private Camera _camera;
    
    private CameraStateMachine _stateMachine;

    private IdleState _idleState;
    private ObjectMoveState _objectMoveState;

    private void Awake()
    {
        _camera = Camera.main;
        
        _stateMachine = new CameraStateMachine();
        InitStates();
        
        _stateMachine.SetState(_idleState);
        
    }

    public void Update()
    {
        _stateMachine.Update();
    }

    public void EnterBuildingMoveState()
    {
        _stateMachine.SetState(_objectMoveState);
    }

    public void EnterIdleState()
    {
        _stateMachine.SetState(_idleState);
    }
    private void InitStates()
    {
        _idleState = new IdleState(
            _camera,
            new PlayerControls(),
            sensitivity,
            Max,
            Min);

        _objectMoveState = new ObjectMoveState(
            _camera,
            new PlayerControls(),
            sensitivity,
            Max,
            Min);
    }
}