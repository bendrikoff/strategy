using Assets.Scripts.Camera;
using Assets.Scripts.Infrastructure;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    public float sensitivity = 1f;
    
    public Vector2 Max = new Vector2(10,5);
    public Vector2 Min = new Vector2(-10, -5);
    
    private CameraStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = new CameraStateMachine();
        
        _stateMachine.SetState(new IdleState(
            Camera.main, 
            new PlayerControls(),
            sensitivity,
            Max,
            Min));
    }

    public void Update()
    {
        _stateMachine.Update();
    }
}