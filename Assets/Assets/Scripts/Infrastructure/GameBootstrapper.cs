using System;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            //_game = new Game();
                
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            //_game.Update();
        }
        
        public void SaveGame()
        {
            //_game.SaveSystem.Save("SceneObjects");
            AllServices.Instance.SaveSystem.Save("SceneObjects");
            Debug.Log("Game saved.");
        }

        public void LoadGame()
        {
            AllServices.Instance.SaveSystem.Load("SceneObjects");
            Debug.Log("Game loaded.");
        }
    }
}