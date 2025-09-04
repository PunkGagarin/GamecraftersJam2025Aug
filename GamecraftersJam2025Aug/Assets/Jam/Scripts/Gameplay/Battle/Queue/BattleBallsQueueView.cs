using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Queue
{
    public class BattleBallsQueueView : MonoBehaviour
    {
        public List<PlayerBallsView> BallsViews { get; set; } = new();

        [field: SerializeField]
        public PlayerBallsView PlayerBallsViewPrefab { get; set; }

        [field: SerializeField]
        public Transform BallsContainer { get; set; }

        public void CreateNewView(BallDto dto)
        {
            var view = Instantiate(PlayerBallsViewPrefab, BallsContainer);
            view.Init(dto);
            BallsViews.Add(view);
        }

        public void InitForBalls(List<BallDto> balls)
        {
            foreach (var dto in balls)
                CreateNewView(dto);
        }

        public void HideBall(int ballId)
        {
            //createAnimation
            BallsViews.Find(b => b.BallId == ballId).gameObject.SetActive(false);
        }

        public void ShowAllBalls()
        {
            foreach (var ball in BallsViews)
                ball.gameObject.SetActive(true);
        }

        public void ReorderBalls(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
                BallsViews.Find(b => b.BallId == ids[i]).transform
                    .SetSiblingIndex(i);
        }

        public void CleanUp()
        {
            foreach (var view in BallsViews)
            {
                Destroy(view.gameObject);
            }
            BallsViews.Clear();
        }
    }
}