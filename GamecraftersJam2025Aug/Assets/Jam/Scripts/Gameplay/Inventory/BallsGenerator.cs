using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Configs;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class BallsGenerator : IInitializable
    {

        [Inject] private BallsConfigRepository _ballsConfigRepository;

        [Inject] private BallDescriptionGenerator _ballDescriptionGenerator;

        private int _ballId;

        public void Initialize()
        {
            _ballsConfigRepository.Check();
        }

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
            var model = new PlayerBallModel(_ballId, ballSo.BallType, ballSo.Grade, ballSo.Sprite, effects);
            _ballId++;
            return model;
        }

        public PlayerBallModel CreateBallFor(BallType type, int grade)
        {
            BallSo ballSo = GetSoByType(type, grade);
            return CreateBallFrom(ballSo);
        }

        private BallSo GetSoByType(BallType type, int grade)
        {
            return _ballsConfigRepository.AllPlayerBalls.FirstOrDefault(b => b.BallType == type && b.Grade == grade);
        }

        public BallsInventoryModel CreateBallsInventoryModel()
        {
            return new BallsInventoryModel();
        }

        public BallRewardDto CreateRandomBallRewardDto()
        {
            BallSo randomSo = GetRandomBallSo();
            return CreateBallRewardDtoFrom(randomSo);
        }

        public BallRewardDto CreateBallRewardDtoFrom(BallType ballType, int grade)
        {
            BallSo ballSo = GetSoByType(ballType, grade);
            return CreateBallRewardDtoFrom(ballSo);
        }

        private BallRewardDto CreateBallRewardDtoFrom(BallSo randomSo)
        {
            var ballRewardDto = new BallRewardDto(randomSo.Sprite, randomSo.Grade, randomSo.Description, randomSo.BallType);
            _ballDescriptionGenerator.AddEffectsDescriptionTo(randomSo.Effects, ballRewardDto);
            return ballRewardDto;
        }

        private BallSo GetRandomBallSo()
        {
            var defaultPlayerBalls = _ballsConfigRepository.DefaultPlayerBalls;
            return defaultPlayerBalls[Random.Range(0, defaultPlayerBalls.Count)];
        }

        public bool CanCreateBallFor(BallType ballType, int ballGrade)
        {
            return GetSoByType(ballType, ballGrade) != null;
        }

        // CreateRandomBallRewardDto -> отрисовываем вью, сетим во вью ТИП (уникальный ID шара)

        // -> из вью приходит событие что юзер собрал шар с таким типом

        // -> создаём модель через CreateBallFrom и в сервисе её добавляем в инвентарь
    }
}