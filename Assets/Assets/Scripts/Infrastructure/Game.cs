using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public ISaveSystem SaveSystem;
        public Game()
        {
            SaveSystem = new PlayerPrefsSaveSystem();
        }
        
        public void Update()
        {
        }
    }
}

