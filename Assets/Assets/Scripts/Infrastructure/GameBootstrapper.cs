using System;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game();
                
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            _game.Update();
        }
    }
}