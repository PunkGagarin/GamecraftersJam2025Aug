using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class ConcreteBallRewardPayload : IRewardPayload
    {
        public ConcreteBallRewardPayload(BallSo ball) => Ball = ball;
        public BallSo Ball { get; set; }
    }
}