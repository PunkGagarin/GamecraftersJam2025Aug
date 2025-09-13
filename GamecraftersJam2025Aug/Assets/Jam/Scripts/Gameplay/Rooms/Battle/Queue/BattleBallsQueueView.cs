using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BattleBallsQueueView : MonoBehaviour
    {
        public List<PlayerBallView> BallsViews { get; set; } = new();

        [field: SerializeField]
        public PlayerBallView PlayerBallsViewPrefab { get; set; }

        [field: SerializeField]
        public Transform BallsContainer { get; set; }
        
                
        public Action<string> OnEnter { get; set; } = delegate { };
        public Action OnExit { get; set; } = delegate { };

        public void CreateNewView(BallDto dto)
        {
            var view = Instantiate(PlayerBallsViewPrefab, BallsContainer);
            view.Init(dto);
            BallsViews.Add(view);
            view.OnEnter += OnEnter;
            view.OnExit += OnExit;
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
                view.OnEnter -= OnEnter;
                view.OnExit -= OnExit;
                Destroy(view.gameObject);
            }
            BallsViews.Clear();
        }
    }
}