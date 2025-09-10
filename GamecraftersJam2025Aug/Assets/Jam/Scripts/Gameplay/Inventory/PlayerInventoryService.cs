using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

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

        public void UpgradeBall(int ballId)
        {
            PlayerBallModel ball = _ballsInventoryModel.Balls.Find(b => b.BallId == ballId);
            Debug.LogError($"Can upgrade ball {ball.Type} {ball.Grade}: {CanUpgradeBall(ball)}");
            if (CanUpgradeBall(ball))
            {
                var newBall = _ballFactory.CreateBallFor(ball.Type, ball.Grade + 1);
                RemoveBall(ball);
                AddBall(newBall);
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
            var ball = _ballsInventoryModel.Balls.Find(b => b.BallId == ballId);
            return new BallBattleDto(ball);
        }

        public void UpgradeRandomBall()
        {
            Debug.LogError(" UpgradeRandomBall");
            var canUpgradeBallList = _ballsInventoryModel.Balls.Where(b => CanUpgradeBall(b)).ToList();

            if (canUpgradeBallList.Count > 0)
            {
                var ball = canUpgradeBallList[UnityEngine.Random.Range(0, canUpgradeBallList.Count)];
                Debug.LogError($" Find ball to upgrade: {ball.BallId} {ball.Type} {ball.Grade}");
                UpgradeBall(ball.BallId);
            }
        }
    }
}