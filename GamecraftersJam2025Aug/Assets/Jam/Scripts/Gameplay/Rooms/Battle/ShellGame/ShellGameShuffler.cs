using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.ShellGame
{
    public class ShellGameShuffler
    {
        private int MinShuffleCount { get; set; }
        private int MaxShuffleCount { get; set; }
        private float NextPairPickupSpeed { get; set; }
        private float CupShuffleSpeed { get; set; }
            
        private (CupView one, CupView two) _currentPair;

        public void Init(ShellGameConfig shellGameConfig)
        {
            MinShuffleCount = shellGameConfig.MinShuffleCount;
            MaxShuffleCount = shellGameConfig.MaxShuffleCount;
            NextPairPickupSpeed = shellGameConfig.NextPairPickupSpeed;
            CupShuffleSpeed = shellGameConfig.CupShuffleSpeed;
        }

        public async UniTask Shuffle(List<CupView> activeCups)
        {
            for (int i = 0; i < Random.Range(MinShuffleCount, MaxShuffleCount); i++)
            {
                PickPair(activeCups);
                await ShufflePair();
                await UniTask.Delay((int)(NextPairPickupSpeed * 1000));
            }
        }
            
        private void PickPair(List<CupView> activeCups)
        {
            var firstCup = activeCups[Random.Range(0, activeCups.Count)];
            var secondCup = activeCups[Random.Range(0, activeCups.Count)];

            while (firstCup == secondCup)
                secondCup = activeCups[Random.Range(0, activeCups.Count)];

            _currentPair = (firstCup, secondCup);
        }

        private async UniTask ShufflePair()
        {
            MoveUtils.MoveOverTime2D(_currentPair.one.transform, _currentPair.two.transform.position,
                CupShuffleSpeed);
            await MoveUtils.MoveOverTime2D(_currentPair.two.transform, _currentPair.one.transform.position,
                CupShuffleSpeed);
        }
    }
}