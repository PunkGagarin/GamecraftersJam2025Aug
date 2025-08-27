using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyView : MonoBehaviour
    {

        [field: SerializeField]
        public TextMeshProUGUI HealthText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI DamageText { get; private set; }

        [field: SerializeField]
        public SpriteRenderer Sprite { get; private set; }

        [field: SerializeField]
        public HpBar Bar { get; private set; }


        public void Init(Sprite enemySprite, int maxHealth)
        {
            Sprite.sprite = enemySprite;
            DamageText.gameObject.SetActive(false);
            Bar.SetHpBarFill(1f);
            SetHealth(maxHealth, maxHealth);
        }

        private void SetHealth(int currentHealth, int maxHealth)
        {
            HealthText.text = $"{currentHealth}/{maxHealth}";
        }

        public void FillHpBarFor(float fill)
        {
            Bar.SetHpBarFill(fill);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
        }

        public void SetDamage(int damage)
        {
        }
    }
}