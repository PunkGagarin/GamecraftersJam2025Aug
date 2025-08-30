using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Shared.Ui
{
    public class BaseUnitView : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI HealthText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI DamageText { get; private set; }

        [field: SerializeField]
        public SpriteRenderer Sprite { get; private set; }

        [field: SerializeField]
        public HpBar Bar { get; private set; }

        private Vector3 _startDamageTextPosition;
        private float _duration = 1f;

        private void Awake()
        {
            _startDamageTextPosition = DamageText.rectTransform.localPosition;
        }

        public virtual void Init(int maxHealth)
        {
            DamageText.gameObject.SetActive(false);
            Bar.SetHpBarFill(1f);
            SetHealth(maxHealth, maxHealth);
        }

        public void SetHealth(int currentHealth, int maxHealth)
        {
            HealthText.text = $"{currentHealth}/{maxHealth}";
            FillHpBarFor((float)currentHealth / maxHealth);
        }

        protected void FillHpBarFor(float fill)
        {
            Bar.SetHpBarFill(fill);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
        }

        public void SetDamageText(int damage)
        {
            DamageText.gameObject.SetActive(true);
            DamageText.text = damage.ToString();
            FadeOutDamageText();
        }

        private void FadeOutDamageText()
        {
            SetDefaultState();
            DamageText.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();

            seq.Append(DamageText.rectTransform.DOLocalMoveY(_startDamageTextPosition.y + 4f, _duration));
            seq.Join(DamageText.DOFade(0f, _duration));

            seq.OnComplete(() =>
            {
                DamageText.gameObject.SetActive(false);
                SetDefaultState();
            });
        }

        private void SetDefaultState()
        {
            DamageText.rectTransform.localPosition = _startDamageTextPosition;
            DamageText.color = new Color(DamageText.color.r, DamageText.color.g, DamageText.color.b, 1f);
        }

        public async Task PlayAttackAnimation()
        {
            await  Task.Delay(1000);
            return;
        }
    }
}