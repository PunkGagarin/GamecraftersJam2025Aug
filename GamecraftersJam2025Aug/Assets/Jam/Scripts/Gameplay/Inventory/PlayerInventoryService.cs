using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryService : IInitializable, IDisposable
    {
        [Inject] private readonly BallsGenerator _ballFactory;
        [Inject] private readonly InventoryBus _inventoryBus;

        private BallsInventoryModel _ballsInventoryModel;

        public void Initialize()
        {
            _ballsInventoryModel = _ballFactory.CreateBallsInventoryModel();
            var defaultBalls = _ballFactory.CreateDefaultBalls();

            foreach (var playerBallModel in defaultBalls)
                AddBall(playerBallModel);
        }

        public void Dispose()
        {
        }
        
        public void CreateAndAddBall(BallType ballType, int grade)
        {
            var ball = _ballFactory.CreateBallFor(ballType, grade);
            AddBall(ball);
        }

        public void AddBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.AddBall(ball);
            var ballDto = CreateBallDto(ball);
            _inventoryBus.BallAddedInvoke(ballDto);
        }

        private static BallDto CreateBallDto(PlayerBallModel ball)
        {
            var ballDto = new BallDto(ball.BallId, ball.Sprite, ball.Type, ball.Grade, ball.Description);
            return ballDto;
        }

        public void AddBalls(List<PlayerBallModel> balls)
        {
            foreach (var ball in balls)
                AddBall(ball);
        }

        public void RemoveBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.RemoveBall(ball);
            var ballDto = CreateBallDto(ball);
            _inventoryBus.BallRemovedInvoke(ballDto);
        }

        public void UpgradeBall(int ballId, out PlayerBallModel upgradedBall)
        {
            upgradedBall = null;
            PlayerBallModel ball = _ballsInventoryModel.Balls.Find(b => b.BallId == ballId);
            if (CanUpgradeBall(ball))
            {
                var newBall = _ballFactory.CreateBallFor(ball.Type, ball.Grade + 1);
                RemoveBall(ball);
                AddBall(newBall);
                upgradedBall = newBall;
            }
        }

        private bool CanUpgradeBall(PlayerBallModel ball)
        {
            return ball.Grade < 2 && _ballFactory.CanCreateBallFor(ball.Type, ball.Grade + 1);
        }

        public List<PlayerBallModel> GetAllBallsCopy()
        {
            return _ballsInventoryModel.Balls.Select(b => b.Clone()).ToList();
        }

        public BallBattleDto GetBattleBallById(int ballId)
        {
            PlayerBallModel ball = _ballsInventoryModel.Balls.Find(b => b.BallId == ballId);
            return new BallBattleDto(ball);
        }

        public void UpgradeRandomBall()
        {
            var canUpgradeBallList = _ballsInventoryModel.Balls.Where(CanUpgradeBall).ToList();

            if (canUpgradeBallList.Count > 0)
            {
                var ball = canUpgradeBallList[Random.Range(0, canUpgradeBallList.Count)];
                UpgradeBall(ball.BallId, out _);
            }
            else
            {
                Debug.Log("Cannot upgrade random ball.");
            }
        }

        public BallRewardCardUiData UpdateRandomPlayerBallWithGrade(int grade, out BallRewardCardUiData upgradedBall)
        {
            var canUpgradeBallList = _ballsInventoryModel.Balls
                .Where(b => CanUpgradeBall(b) && b.Grade == grade )
                .ToList();
            
            if (canUpgradeBallList.Count > 0)
            {
                PlayerBallModel prevBallModel = canUpgradeBallList[Random.Range(0, canUpgradeBallList.Count)];
                UpgradeBall(prevBallModel.BallId, out var newBall);
                var newBallModel = newBall;
                upgradedBall = new BallRewardCardUiData(newBallModel.Sprite, newBallModel.Description, newBallModel.Type, newBallModel.Grade);
                return new BallRewardCardUiData(prevBallModel.Sprite, prevBallModel.Description, prevBallModel.Type, prevBallModel.Grade);
            }

            Debug.Log("Cannot upgrade random ball.");
            upgradedBall = null;
            return null;
        }

        public PlayerBallModel GetRandomPlayerBall()
        {
            var index = Random.Range(0, _ballsInventoryModel.Balls.Count);
            var randomBall = _ballsInventoryModel.Balls[index];
            return randomBall;
        }
        
        public PlayerBallModel GetPlayerBall(BallType ballType, int grade) => 
            _ballsInventoryModel.Balls.First(b => b.Type == ballType && b.Grade == grade);
    }
}