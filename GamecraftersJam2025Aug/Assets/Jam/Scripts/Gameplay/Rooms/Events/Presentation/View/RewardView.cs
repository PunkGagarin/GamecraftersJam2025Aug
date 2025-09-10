using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public abstract class RewardView : MonoBehaviour
    {
        public abstract void SetData(IRewardCardUiData data);
    }
}