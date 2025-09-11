using System;
using UnityEngine;
using Zenject;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldPresenter : IInitializable, IDisposable
    {
        [Inject] private GoldUI _view;
        [Inject] private GoldBus _bus;

        public void Initialize()
        {
            _bus.OnGoldChanged += UpdateUi;
            _bus.OnGoldInit += SetGoldUi;
        }

        public void Dispose()
        {
            _bus.OnGoldChanged -= UpdateUi;
            _bus.OnGoldInit -= SetGoldUi;
        }

        private void SetGoldUi(int gold)
        {
            _view.SetGoldUi(gold);
        }

        private void UpdateUi(int diff, int newTotal)
        {
            _view.UpdateUi(diff, newTotal);
        }
    }
}