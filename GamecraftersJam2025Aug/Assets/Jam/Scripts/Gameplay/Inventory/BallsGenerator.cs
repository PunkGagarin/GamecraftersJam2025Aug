using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Configs;
using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class BallsGenerator
    {
        [Inject] private BallsConfigRepository _ballsConfigRepository;
        [Inject] private BallsInventoryModel _ballsInventoryModel;
        // move into repository?

        public void CreateDefaultBalls()
        {
            var defaultBalls = new List<PlayerBallModel>();
            foreach (var ballSo in _ballsConfigRepository.DefaultPlayerBalls)
            {
                var ball = CreateBallFrom(ballSo);
                defaultBalls.Add(ball);
            }
            _ballsInventoryModel.Init(defaultBalls);
        }

        private PlayerBallModel CreateBallFrom(BallSo ballSo)
        {
            
            var model = new PlayerBallModel(ballSo.Damage);
            return model;
        }

        public BallsInventoryModel CreateBallsInventoryModel()
        {
            return new BallsInventoryModel();
        }
    }
}