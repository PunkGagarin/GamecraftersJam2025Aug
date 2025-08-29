using Jam.Scripts.Gameplay.Battle.Shared.Ui;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Player.Ui
{
    public class PlayerBattleView : BaseUnitView
    {
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
    }
}