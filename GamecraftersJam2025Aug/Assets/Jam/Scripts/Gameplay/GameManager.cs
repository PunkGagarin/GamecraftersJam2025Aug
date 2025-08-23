using System;
using Jam.Scripts.Gameplay.Inventory;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private BallsGenerator _ballsGenerator;

        private void Awake()
        {
            StartGame();
        }

        public void StartGame()
        {
            CreateMap();
            CreatePlayerBalls();
            //ShowDefaultScene
        }
        
        private void CreatePlayerBalls()
        {
            _ballsGenerator.CreateDefaultBalls();
        }

        private void CreateMap()
        {
            //call generate map func
        }
    }

}