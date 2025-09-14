using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Shared.Ui
{
    public class BaseUnitView : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI HealthText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI DamageText { get; private set; }
        
        [field: SerializeField]
        public TextMeshProUGUI HealText { get; private set; }

        [field: SerializeField]
        public HpBar Bar { get; private set; }

        private Vector3 _startDamageTextPosition;
        private Vector3 _startHealTextPosition;
        private float _duration = 1f;

        public virtual void Awake()
        {
            _startDamageTextPosition = DamageText.rectTransform.localPosition;
            _startHealTextPosition = HealText.rectTransform.localPosition;
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
            SetDefaultDamageState();
            DamageText.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();

            seq.Append(DamageText.rectTransform.DOLocalMoveY(_startDamageTextPosition.y + 4f, _duration));
            seq.Join(DamageText.DOFade(0f, _duration));

            seq.OnComplete(() =>
            {
                DamageText.gameObject.SetActive(false);
                SetDefaultDamageState();
            });
        }

        private void SetDefaultDamageState()
        {
            DamageText.rectTransform.localPosition = _startDamageTextPosition;
            DamageText.color = new Color(DamageText.color.r, DamageText.color.g, DamageText.color.b, 1f);
        }

        public void SetHealText(int damage)
        {
            HealText.gameObject.SetActive(true);
            HealText.text = damage.ToString();
            FadeOutHealText();
        }

        private void FadeOutHealText()
        {
            SetDefaultHealState();
            HealText.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();

            seq.Append(HealText.rectTransform.DOLocalMoveY(_startHealTextPosition.y + 4f, _duration));
            seq.Join(HealText.DOFade(0f, _duration));

            seq.OnComplete(() =>
            {
                HealText.gameObject.SetActive(false);
                SetDefaultHealState();
            });
        }
        private void SetDefaultHealState()
        {
            HealText.rectTransform.localPosition = _startHealTextPosition;
            HealText.color = new Color(HealText.color.r, HealText.color.g, HealText.color.b, 1f);
        }
    }
}