using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Inventory.Views
{
    public class PlayerBallWithUpgradeView : PlayerBallView
    {
        [field: SerializeField]
        public Button UpgradeButton { get; private set; }

        public Action<BallDto> OnBallUpgradeClicked { get; set; }

        private void Awake()
        {
            UpgradeButton.onClick.AddListener(OnBallClicked);
        }

        private void OnBallClicked()
        {
            OnBallUpgradeClicked?.Invoke(Dto);
        }

        private void OnDestroy()
        {
            UpgradeButton.onClick.RemoveListener(OnBallClicked);
        }


        public void TurnOnUpgrade()
        {
            UpgradeButton.enabled = true;
        }

        public void TurnOffUpgrade()
        {
            UpgradeButton.enabled = false;
        }
    }
}