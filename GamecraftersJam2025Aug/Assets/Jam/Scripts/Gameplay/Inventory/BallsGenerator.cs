using System.Collections.Generic;
using System.Linq;
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
            var effects = ballSo.Effects.Select(e => e.ToInstance()).ToList();
            var model = new PlayerBallModel(_ballId, ballSo.Sprite, effects);
            _ballId++;
            return model;
        }

        public BallsInventoryModel CreateBallsInventoryModel()
        {
            return new BallsInventoryModel();
        }
    }
}