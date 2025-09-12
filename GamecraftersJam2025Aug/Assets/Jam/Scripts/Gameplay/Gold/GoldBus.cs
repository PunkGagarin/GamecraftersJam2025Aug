using System;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldBus
    {
        public Action<int, int> OnGoldChanged = delegate { };
        public Action<int> OnGoldInit = delegate { };
    }
}