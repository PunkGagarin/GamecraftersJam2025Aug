using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class InventoryBus
    {
        public event Action<BallDto> OnBallAdded = delegate { };
        public event Action<BallDto> OnBallRemoved = delegate { };
        public event Action OnBallUpgraded = delegate { };
        public event Action<List<BallDto>> OnInited = delegate { };

        public void BallAddedInvoke(BallDto ball) => OnBallAdded.Invoke(ball);
        public void BallRemovedInvoke(BallDto ball) => OnBallRemoved.Invoke(ball);
        public void BallUpgradedInvoke() => OnBallUpgraded.Invoke();
        public void InitedInvoke(List<BallDto> balls) => OnInited.Invoke(balls);
    }
}