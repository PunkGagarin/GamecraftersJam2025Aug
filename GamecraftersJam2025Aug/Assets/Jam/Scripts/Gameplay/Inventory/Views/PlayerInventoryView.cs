using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Inventory.Views
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
        public PlayerBallWithUpgradeView BallViewPrefab { get; private set; }

        [field: SerializeField]
        public Button TestUpgradeButton { get; private set; }

        private List<PlayerBallWithUpgradeView> _views = new();

        public Action<BallDto> OnBallUpgradeClicked { get; set; }


        public void AddBall(BallDto dto)
        {
            PlayerBallWithUpgradeView ballView = Instantiate(BallViewPrefab, BallsContainer);
            ballView.OnBallUpgradeClicked += OnBallUpgradeClicked;
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
            _views.Remove(ball);
            ball.OnBallUpgradeClicked -= OnBallUpgradeClicked;
            Destroy(ball.gameObject);
        }

        public void TurnOnUpgrade()
        {
            foreach (var view in _views)
            {
                view.gameObject.SetActive(view.Dto.Grade < 2);
                view.TurnOnUpgrade();
            }
        }

        public override void Show()
        {
            base.Show();
            foreach (var view in _views)
            {
                view.gameObject.SetActive(true);
                view.TurnOffUpgrade();
            }
        }
    }
}