using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Configs;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
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

        private PlayerBallModel CreateBallFrom(BallSo ballSo)
        {
            var effects = ballSo.Effects.Select(e => e.ToInstance()).ToList();
            var model = new PlayerBallModel(_ballId, ballSo.Sprite, effects);
            _ballId++;
            return model;
        }
        
        private PlayerBallModel CreateBallFrom(BallType type)
        {
            BallSo ballSo = GetSoByType(type);
            var effects = ballSo.Effects.Select(e => e.ToInstance()).ToList();
            var model = new PlayerBallModel(_ballId, ballSo.Sprite, effects);
            _ballId++;
            return model;
        }

        private BallSo GetSoByType(BallType type)
        {
            return _ballsConfigRepository.AllPlayerBalls.FirstOrDefault( b => b.BallType == type);
        }

        public BallsInventoryModel CreateBallsInventoryModel()
        {
            return new BallsInventoryModel();
        }
        
        public BallRewardDto CreateRandomBallRewardDto()
        {
            BallSo randomSo = GetRandomBallSo();
            return new BallRewardDto(randomSo.BallType, randomSo.Sprite, randomSo.Description);
        }

        private BallSo GetRandomBallSo()
        {
            var defaultPlayerBalls = _ballsConfigRepository.DefaultPlayerBalls;
            return defaultPlayerBalls[Random.Range(0, defaultPlayerBalls.Count)];
        }
        
        // CreateRandomBallRewardDto -> отрисовываем вью, сетим во вью ТИП (уникальный ID шара)
        // -> из вью приходит событие что юзер собрал шар с таким типом
        // -> создаём модель через CreateBallFrom и в сервисе её добавляем в инвентарь
    }
}