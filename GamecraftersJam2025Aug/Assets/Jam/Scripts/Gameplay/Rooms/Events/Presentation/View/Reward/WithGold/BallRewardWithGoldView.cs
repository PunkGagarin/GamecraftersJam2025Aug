using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation.WithGold
{
    public class BallRewardWithGoldView : RewardCardView
    {
        
        [field: SerializeField] 
        public TextMeshProUGUI GoldAmountText { get; private set; }
        
        public void SetGoldAmount(int goldAmount)
        {
            GoldAmountText.text = goldAmount.ToString();
        }
    }
}