using Assets.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public ISaveSystem SaveSystem;
        public PlayStateMachine PlayStateMachine;
        public Game()
        {
            SaveSystem = new PlayerPrefsSaveSystem();
            InitPlayState();
        }
        
        public void Update()
        {
            PlayStateMachine.Update();
        }

        private void InitPlayState()
        {
            PlayStateMachine = new PlayStateMachine();
            var playingState = new PlayingPlayState(BuildingReferences.Instance.CameraController);
            var buildingState = new BuildingPlayState(BuildingReferences.Instance.GridData, BuildingReferences.Instance.CameraController);
            
            PlayStateMachine.SetState(playingState);

            UIEvents.OnStartBuilding += () => PlayStateMachine.SetState(buildingState);
            UIEvents.OnEndBuilding += () => PlayStateMachine.SetState(playingState);
        }
    }
}

