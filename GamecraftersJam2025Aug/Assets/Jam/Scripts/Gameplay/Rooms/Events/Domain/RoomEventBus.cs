using System;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventBus
    {
        public event Action<DealUiData> OnStartDealEvent = delegate { };
        public event Action<RewardUiData> OnStartRewardEvent = delegate { };

        public void StartDealEvent(DealUiData data) => OnStartDealEvent.Invoke(data);
        public void StartRewardEvent(RewardUiData data) => OnStartRewardEvent.Invoke(data);
    }
}