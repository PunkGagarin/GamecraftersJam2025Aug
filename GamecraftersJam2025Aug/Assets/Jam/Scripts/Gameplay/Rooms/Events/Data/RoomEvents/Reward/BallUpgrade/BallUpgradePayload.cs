using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class BallUpgradePayload : IRewardPayload
    {
        public BallType BallType { get; set; }

        public BallUpgradePayload(BallType ballType) => BallType = ballType;
    }
}