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
            PlayStateMachine = new PlayStateMachine();
        }
        
        public void Update()
        {
            PlayStateMachine.Update();
        }
    }
}

