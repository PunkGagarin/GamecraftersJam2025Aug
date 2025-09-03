using System;
using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class MapPlayerPrefab : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public void Setup(Vector2 startPos)
        {
            gameObject.SetActive(false);
            _rectTransform.anchoredPosition = startPos;
        }

        public void SetAndAnimatePos(Vector2 position, float cellSize, Action onComplete = null)
        {
            gameObject.SetActive(true);
            AnimateMove(position, cellSize, onComplete);
        }

        private void AnimateMove(Vector2 targetPos, float cellSize, Action onComplete)
        {
            Vector2 startPos = _rectTransform.anchoredPosition;
            float peakHeight = cellSize / 2;
            float t = 0f;

            DOTween.To(() => t, x =>
            {
                t = x;
                float xPos = Mathf.Lerp(startPos.x, targetPos.x, t);
                float yPos = Mathf.Lerp(startPos.y, targetPos.y, t) + peakHeight * 4 * t * (1 - t);
                _rectTransform.anchoredPosition = new Vector2(xPos, yPos);
            }, 1f, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => DOVirtual.DelayedCall(0.5f, () => onComplete?.Invoke()));
        }
    }
}