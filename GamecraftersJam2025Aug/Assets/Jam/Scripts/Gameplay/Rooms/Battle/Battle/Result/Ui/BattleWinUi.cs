using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation.WithGold;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleWinUi : ContentUi
    {

        [field: SerializeField]
        public Button ToMapButton { get; private set; }

        [field: SerializeField]
        public Button UpgradeButton { get; private set; }

        [field: SerializeField]
        public HealWinButton HealButton { get; private set; }

        [field: SerializeField]
        public List<BallRewardWithGoldView> BallBuyViews { get; private set; }


        public void InitWinData(WinDto winDto)
        {
            for (int i = 0; i < BallBuyViews.Count; i++)
            {
                BallBuyViews[i].SetData(winDto.Balls[i]);
                BallBuyViews[i].SetGoldAmount(winDto.Balls[i].GoldPrice);
            }
            
            HealButton.HealButton.interactable = true;
        }
    }

}