using System;
using Jam.Scripts.Gameplay.Inventory.Views;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryPresenter : IInitializable, IDisposable
    {
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly InventoryBus _bus;
        [Inject] private readonly PlayerInventoryView _view;


        public void Initialize()
        {
            _bus.OnBallAdded += AddBall;
            _bus.OnBallRemoved += RemoveBall;
            _view.OpenButton.onClick.AddListener(_view.Show);
            _view.CloseButton.onClick.AddListener(_view.Hide);
            _view.OnBallUpgradeClicked += UpgradeBall;
            // _view.TestUpgradeButton.onClick.AddListener(TestUpgrade);
        }

        public void Dispose()
        {
            _bus.OnBallAdded -= AddBall;
            _bus.OnBallRemoved -= RemoveBall;
            _view.OpenButton.onClick.RemoveListener(_view.Show);
            _view.CloseButton.onClick.RemoveListener(_view.Hide);
            _view.TestUpgradeButton.onClick.RemoveListener(TestUpgrade);
            _view.OnBallUpgradeClicked -= UpgradeBall;
        }

        private void UpgradeBall(BallDto dto)
        {
            //todo: remove out??
            _playerInventoryService.UpgradeBall(dto.Id, out _);
            _view.Hide();
        }

        private void RemoveBall(BallDto dto)
        {
            _view.RemoveBall(dto.Id);
        }

        private void AddBall(BallDto dto)
        {
            _view.AddBall(dto);
        }

        private void TestUpgrade()
        {
            _playerInventoryService.UpgradeRandomBall();
        }

        public void OpenUpgrade()
        {
            _view.Show();
            _view.TurnOnUpgrade();
        }
    }
}