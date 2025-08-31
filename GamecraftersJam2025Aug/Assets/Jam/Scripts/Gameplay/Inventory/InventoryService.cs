using System;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class InventoryService : IInitializable, IDisposable
    {
        [Inject] private InventoryBus _inventoryBus;
        
        private BallsInventoryModel _ballsInventoryModel = new();

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        public void AddBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.AddBall(ball);
            _inventoryBus.BallAddedInvoke(ball);
        }
                
        public void RemoveBall(PlayerBallModel ball)
        {
            _ballsInventoryModel.AddBall(ball);
            _inventoryBus.BallRemovedInvoke(ball);
        }

        public void UpgradeBall(int ballId)
        {
            
            
        }
        
        
    }
}