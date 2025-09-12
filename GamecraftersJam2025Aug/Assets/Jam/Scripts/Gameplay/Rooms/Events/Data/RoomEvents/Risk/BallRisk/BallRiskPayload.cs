using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class BallRiskPayload : IRiskPayload
    {
        public BallSo Ball { get; set; }
        public BallRiskPayload(BallSo ball) => Ball = ball;
    }
}