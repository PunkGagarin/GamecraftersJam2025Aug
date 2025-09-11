using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public abstract class DealView : MonoBehaviour
    {
        public abstract void SetData(IRewardCardUiData data);
    }
}