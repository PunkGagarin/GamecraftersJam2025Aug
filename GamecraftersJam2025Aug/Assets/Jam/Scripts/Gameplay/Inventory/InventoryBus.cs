using System;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class InventoryBus
    {
        public event Action<PlayerBallModel> OnBallAdded = delegate { };
        public event Action<PlayerBallModel> OnBallRemoved = delegate { };
        public event Action<PlayerBallModel> OnBallUpgraded = delegate { };

        public void BallAddedInvoke(PlayerBallModel ball) => OnBallAdded.Invoke(ball);
        public void BallRemovedInvoke(PlayerBallModel ball) => OnBallRemoved.Invoke(ball);
        public void BallUpgradedInvoke(PlayerBallModel ball) => OnBallUpgraded.Invoke(ball);
    }
}