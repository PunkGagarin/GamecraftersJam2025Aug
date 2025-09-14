using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Battle.Shared.Ui;
using Jam.Scripts.UI;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerBattleView : BaseUnitView
    {
        [field: SerializeField]
        private Transform PlayerQueueTransform { get; set; }

        [field: SerializeField]
        private List<PlayerBallView> QueueBallViews { get; set; }
        
        [field: SerializeField]
        private PlayerGraphic PlayerGraphic { get; set; }
        
        public Action<string> OnEnter { get; set; } = delegate { };
        public Action OnExit { get; set; } = delegate { };

        public override void Awake()
        {
            base.Awake();
            foreach (var ballView in QueueBallViews)
            {
                ballView.OnEnter += OnEnterInvoke;
                ballView.OnExit += OnExitInvoke;
            }
        }

        private void OnDestroy()
        {
            foreach (var ballView in QueueBallViews)
            {
                ballView.OnEnter -= OnEnter;
                ballView.OnExit -= OnExit;
            }
        }

        private void OnEnterInvoke(string obj)
        {
            OnEnter(obj);
        }

        private void OnExitInvoke()
        {
            OnExit();
        }

        public void ShowHeal(int currentHealth, int maxHealth, int parametersHeal)
        {
            SetHealth(currentHealth, maxHealth);
        }

        public void ShowDamageTaken(int currentHealth, int maxHealth, int damage)
        {
            SetHealth(currentHealth, maxHealth);
            SetDamageText(damage);
            PlayDamageAnimation();
        }

        private void PlayDamageAnimation()
        {
            _ = PlayerGraphic.TakeDamage();
            Debug.Log("player TakeDamage");
        }

        public void ShowDeath()
        {
            _ = PlayerGraphic.Death();
            Debug.Log("player death");
        }

        public void TurnOffAllQueueBalls()
        {
            foreach (var ballView in QueueBallViews)
            {
                ballView.gameObject.SetActive(false);
            }
        }

        public void TurnOnQueueBall(BallDto ballDto)
        {
            var ballView = QueueBallViews.FirstOrDefault(el => !el.gameObject.activeSelf);
            if (ballView == null)
            {
                Debug.LogError("Some problem with player little queue");
                return;
            }

            ballView.gameObject.SetActive(true);
            ballView.Init(ballDto);
        }


        public void TurnOffLastBall()
        {
            var ballView = QueueBallViews.FirstOrDefault(el => el.gameObject.activeSelf);
            if (ballView != null)
            {
                ballView.gameObject.SetActive(false);
            }
        }
        
            
        public override async UniTask PlayAttackAnimation()
        {
            await PlayerGraphic.Attack();
            Debug.Log("player Attack");
        }
    }
}