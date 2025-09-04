using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellCreator
    {
        private CupView _cupViewPrefab;
        private BoardBallView _greenBallViewPrefab;
        private BoardBallView _redBallViewPrefab;
            
        public void ParseConfig(ShellGameConfig shellGameConfig)
        {
            _cupViewPrefab = shellGameConfig.CupViewPrefab;
            _greenBallViewPrefab = shellGameConfig.GreenBallViewPrefab;
            _redBallViewPrefab = shellGameConfig.RedBallViewPrefab;
        }
            
        public void CreateNeededObjects(int ballsCount, ShellGameCupAndBallInfo config, List<CupView> sharedList, List<BoardBallView> boardBallViews)
        {
            TurnOnOrCreate(config.CupCount, sharedList, sharedList, _cupViewPrefab);

            var playerBalls = boardBallViews.FindAll(b => b.UnitType == BallUnitType.Player);
            TurnOnOrCreate(ballsCount, boardBallViews, playerBalls, _greenBallViewPrefab);

            var enemyBalls = boardBallViews.FindAll(b => b.UnitType == BallUnitType.Enemy);
            TurnOnOrCreate(config.RedBallCount, boardBallViews, enemyBalls, _redBallViewPrefab);
        }
            
            
        private void TurnOnOrCreate<T>(int count, List<T> sharedList, List<T> specList, T prefab)
            where T : MonoBehaviour
        {
            if (specList.Count < count)
                CreateAndAddInstance(count - specList.Count, prefab, sharedList);

            for (int i = 0; i < specList.Count; i++)
            {
                var ball = specList[i];
                ball.gameObject.SetActive(i < count);
            }
        }

        private void CreateAndAddInstance<T>(int maxCount, T viewPrefab, List<T> listToAdd)
            where T : MonoBehaviour
        {
            for (int i = 0; i < maxCount; i++)
            {
                var cupView = Object.Instantiate(viewPrefab, Vector3.zero, Quaternion.identity);
                listToAdd.Add(cupView);
            }
        }
    }
}