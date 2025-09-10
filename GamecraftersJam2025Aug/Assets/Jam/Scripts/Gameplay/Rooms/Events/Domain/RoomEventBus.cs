using System;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventBus
    {
        public event Action<DealUiData> OnStartDealEvent = delegate { };
        public event Action<RewardUiData> OnStartRewardEvent = delegate { };
        public event Action<BallType> OnBallSelected = delegate { };
        public event Action OnEventFinished = delegate { };

        public void StartDealEvent(DealUiData data) => OnStartDealEvent.Invoke(data);
        public void StartRewardEvent(RewardUiData data) => OnStartRewardEvent.Invoke(data);
        public void BallSelected(BallType type) => OnBallSelected.Invoke(type);
        public void EventFinished() => OnEventFinished();
    }
}