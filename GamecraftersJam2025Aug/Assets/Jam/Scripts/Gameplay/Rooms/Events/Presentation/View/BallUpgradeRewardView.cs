using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallUpgradeRewardView : RewardView
    {
        public event Action OpenBallUpgradePopup = delegate {};
        
        [SerializeField] private Button _button;

        private void Awake() => _button.onClick.AddListener(OnClick);

        private void OnClick() => OpenBallUpgradePopup.Invoke();

        public override void SetData(IRewardCardUiData cardData) {}
        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);
    }
}