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
        
        private int _ballId;

        public List<PlayerBallModel> CreateDefaultBalls()
        {
            var defaultBalls = new List<PlayerBallModel>();
            foreach (var ballSo in _ballsConfigRepository.DefaultPlayerBalls)
            {
                var ball = CreateBallFrom(ballSo);
                defaultBalls.Add(ball);
            }
            return defaultBalls;
        }

        public PlayerBallModel CreateBallFrom(BallSo ballSo)
        {
            var model = new PlayerBallModel(_ballId, ballSo.Damage, ballSo.TargetType, ballSo.Sprite);
            _ballId++;
            return model;
        }

        public BallsInventoryModel CreateBallsInventoryModel()
        {
            return new BallsInventoryModel();
        }
    }
}