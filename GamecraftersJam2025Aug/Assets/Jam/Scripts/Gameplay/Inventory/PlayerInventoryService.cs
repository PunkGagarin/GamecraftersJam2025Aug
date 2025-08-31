using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryService : IInitializable, IDisposable
    {
        [Inject] private readonly BallsGenerator _inventoryFactory;
        [Inject] private readonly InventoryBus _inventoryBus;

        private BallsInventoryModel _ballsInventoryModel;
        private ArtefactsInventoryModel _artefactsInventoryModel;

        public void Initialize()
        {
            _ballsInventoryModel = _inventoryFactory.CreateBallsInventoryModel();
            var defaultBalls = _inventoryFactory.CreateDefaultBalls();
            _ballsInventoryModel.AddBalls(defaultBalls);
            
            _artefactsInventoryModel = new();
        }

        public void Dispose()
        {
        }

        public void AddBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.AddBall(ball);
            _inventoryBus.BallAddedInvoke(ball);
        }

        public void AddBalls(List<PlayerBallModel> balls)
        {
            foreach (var ball in balls)
                AddBall(ball);
        }

        public void RemoveBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.AddBall(ball);
            _inventoryBus.BallRemovedInvoke(ball);
        }

        public void UpgradeBall(int ballId)
        {
            // todo: finish me>
        }

        public List<PlayerBallModel> GetAllBallsCopy()
        {
            return _ballsInventoryModel.Balls.Select(b => b.Clone()).ToList();
        }

        public BallBattleDto GetBattleBallById(int ballId)
        {
            var neededModel = _ballsInventoryModel.Balls.Find(b => b.BallId == ballId);
            return new BallBattleDto(neededModel.Damage, neededModel.TargetType);
        }
    }
}