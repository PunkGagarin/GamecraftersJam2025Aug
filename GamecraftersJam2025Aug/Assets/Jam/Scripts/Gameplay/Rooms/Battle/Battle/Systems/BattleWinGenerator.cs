using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Systems
{
    public class BattleWinGenerator
    {

        [Inject] private BallsGenerator _ballsGenerator;
        [Inject] private BattleWinConfig _winConfig;

        public WinDto GenerateWinData()
        {
            List<BallRewardCardUiData> balls = new List<BallRewardCardUiData>();
            for (int i = 0; i < 3; i++)
            {
                BallRewardCardUiData ball = _ballsGenerator.CreateRandomBallWithGoldRewardDto();
                balls.Add(ball);
            }
            
            
            return new WinDto { Balls = balls, HealAmount = _winConfig.HealAmountPercent, HealCost = _winConfig.HealAmountPercent };
        }
    }
}