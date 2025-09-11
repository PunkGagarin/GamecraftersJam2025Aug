using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryView : ContentUi
    {
        [field: SerializeField]
        public Button OpenButton { get; private set; }

        [field: SerializeField]
        public Button CloseButton { get; private set; }

        [field: SerializeField]
        public Transform BallsContainer { get; private set; }

        [field: SerializeField]
        public PlayerBallView BallViewPrefab { get; private set; }

        [field: SerializeField]
        public Button TestUpgradeButton { get; private set; }

        private List<PlayerBallView> _views = new();


        public void AddBall(BallDto dto)
        {
            var ballView = Instantiate(BallViewPrefab, BallsContainer);
            ballView.Init(dto);
            _views.Add(ballView);
        }

        public void RemoveBall(int dtoId)
        {
            var ball = _views.Find(b => b.BallId == dtoId);
            if (ball == null)
            {
                Debug.LogError("cant find ball with id: " + dtoId);
                return;
            }

            Destroy(ball.gameObject);
        }
    }
}