using System;
using UnityEngine;
using Zenject;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldService : IInitializable
    {
        [Inject] private GoldBus _bus;

        private GoldModel _model;

        public void Initialize()
        {
            _model = new GoldModel();
            _bus.OnGoldInit(0);
        }

        public void AddGold(int amount)
        {
            _model.CurrentGold += amount;
            _bus.OnGoldChanged(amount, _model.CurrentGold);
        }

        public void RemoveGold(int amount)
        {
            _model.CurrentGold -= amount;
            _bus.OnGoldChanged(amount, _model.CurrentGold);
        }
    }

}