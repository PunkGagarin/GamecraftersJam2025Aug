using System.Collections.Generic;
using System.Linq;
using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Configs;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class BallsGenerator : IInitializable
    {
        [Inject] private BallsConfigRepository _ballsConfigRepository;
        [Inject] private BallDescriptionGenerator _ballDescriptionGenerator;
        [Inject] private GoldConfig _goldConfig;

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
            _ballDescriptionGenerator.AddEffectsDescriptionTo(effects, model);
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

        public BallRewardCardUiData CreateRandomBallRewardDto()
        {
            BallSo randomSo = GetRandomBallSo();
            return CreateBallRewardDtoFrom(randomSo);
        }

        public BallRewardCardUiData CreateRandomBallWithGoldRewardDto()
        {
            BallSo randomSo = GetRandomBallSo();
            var dto = CreateBallRewardDtoFrom(randomSo);
            int price = _goldConfig.GetPriceByTypeAndGrade(dto.Grade);
            dto.GoldPrice = price;
            return dto;
        }

        public BallRewardCardUiData CreateBallRewardDtoFrom(BallType ballType, int grade)
        {
            BallSo ballSo = GetSoByType(ballType, grade);
            if (ballSo == null)
            {
                Debug.LogError($"Random ball reward could not be found for {ballType} with grade {grade}");
            }

            return CreateBallRewardDtoFrom(ballSo);
        }

        private BallRewardCardUiData CreateBallRewardDtoFrom(BallSo randomSo)
        {
            var ballRewardDto = new BallRewardCardUiData(randomSo.Sprite, randomSo.Description, randomSo.BallType,
                randomSo.Grade);
            _ballDescriptionGenerator.AddEffectsDescriptionTo(randomSo.Effects, ballRewardDto);
            return ballRewardDto;
        }

        private BallSo GetRandomBallSo()
        {
            var alLBalls = _ballsConfigRepository.AllPlayerBalls;

            int gradeToFind = 0;

            int secondGradePercent = 10;
            if (Random.Range(0, 100) < secondGradePercent)
                gradeToFind = 2;
            else
                gradeToFind = 1;

            alLBalls = alLBalls.Where(b => b.Grade == gradeToFind).ToList();

            var randomBallSo = alLBalls[Random.Range(0, alLBalls.Count)];
            if (randomBallSo == null)
            {
                Debug.LogError(
                    $"Random ball reward could not be found gradeToFind {gradeToFind} secondGradePercent {secondGradePercent} alLBalls.Count {alLBalls.Count}");
            }

            return randomBallSo;
        }

        public bool CanCreateBallFor(BallType ballType, int ballGrade)
        {
            return GetSoByType(ballType, ballGrade) != null;
        }
    }
}