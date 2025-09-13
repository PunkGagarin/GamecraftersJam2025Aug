using System;
using Jam.Scripts.Gameplay.Battle.Enemy;
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
            Debug.Log($" получаем {amount} золота");
            _model.CurrentGold += amount;
            _bus.OnGoldChanged(amount, _model.CurrentGold);
        }

        public void RemoveGold(int amount)
        {
            if (amount > _model.CurrentGold)
                Debug.LogError($" пытаемся снять больше золота чем есть!!! {amount} > {_model.CurrentGold}");

            _model.CurrentGold -= amount;
            _bus.OnGoldChanged(amount, _model.CurrentGold);
        }

        public bool HasGold(int amount)
        {
            return _model.CurrentGold >= amount;
        }
    }
}