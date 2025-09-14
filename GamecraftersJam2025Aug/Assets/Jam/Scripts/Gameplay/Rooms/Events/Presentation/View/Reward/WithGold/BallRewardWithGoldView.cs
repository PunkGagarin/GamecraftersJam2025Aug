using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation.WithGold
{
    public class BallRewardWithGoldView : RewardCardView
    {
        [field: SerializeField]
        public TextMeshProUGUI GoldAmountText { get; private set; }

        [field: SerializeField]
        public Button BackgroundButton { get; private set; }

        [field: SerializeField]
        public Image DisabledBg { get; private set; }
        
        private bool _isBought;

        public void SetIsBought(bool isBought) => _isBought = isBought;

        public void SetGoldAmount(int goldAmount)
        {
            GoldAmountText.text = goldAmount.ToString();
        }

        public void SetInteractable(bool isInteractable)
        {
            if (!_isBought)
            {
                DisabledBg.gameObject.SetActive(!isInteractable);
                BackgroundButton.interactable = isInteractable;
            }
        }

        public bool IsFirstGrade()
        {
            return ((BallRewardCardUiData)_data).Grade == 1;
        }

        public bool IsInteractable() => BackgroundButton.interactable;
    }
}