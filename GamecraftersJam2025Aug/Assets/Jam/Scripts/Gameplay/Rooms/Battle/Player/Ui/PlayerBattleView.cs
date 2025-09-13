using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Battle.Shared.Ui;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerBattleView : BaseUnitView
    {
        [field: SerializeField]
        private Transform PlayerQueueTransform { get; set; }
        
        [field: SerializeField]
        private List<PlayerBallView> QueueBallViews { get; set; }
        
        public void ShowHeal(int currentHealth, int maxHealth, int parametersHeal)
        {
            SetHealth(currentHealth, maxHealth);
        }

        public void ShowDamageTaken(int currentHealth, int maxHealth, int damage)
        {
            SetHealth(currentHealth, maxHealth);
            SetDamageText(damage);
        }

        public void ShowDeath()
        {
            Debug.LogError("Showing player death (no sprite yet");
        }

        public void TurnOffAllQueueBalls()
        {
            foreach (var ballView in QueueBallViews)
            {
                ballView.gameObject.SetActive(false);
            }
        }
        
        // public void TurnOnQueueBall
        
        
    }
}