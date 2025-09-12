using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.UI
{
    public class ClownMonologueController : ContentUi
    {
        [SerializeField] private Image _bubbleImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _duration = .5f;
        [SerializeField] private float _moveUpDistance = 50f;
        [SerializeField] private Button _nextButton;
        [SerializeField] private float _showTime;

        private List<string> _messages = new();
        private int _currentMessageIndex;

        private Sequence _showSequence;

        private void Awake() => _nextButton.onClick.AddListener(OnNextClicked);

        private void OnNextClicked()
        {
            HideText();
        }

        public void ShowTextWithTimer(string message)
        {
            if (_messages != null)
            {
                _messages = new List<string>();
                _currentMessageIndex = 0;
                HideNextButton();
            }


            _text.text = message;
            _text.alpha = 0f;

            Show();
            _bubbleImage.color = new Color(_bubbleImage.color.r, _bubbleImage.color.g, _bubbleImage.color.b, 0f);

            Vector2 startPos = _bubbleImage.rectTransform.anchoredPosition;

            _bubbleImage.rectTransform.localScale = Vector3.zero;
            _bubbleImage.rectTransform.anchoredPosition = startPos;

            Sequence seq = DOTween.Sequence();

            var duration = .4f;
            seq.Append(_bubbleImage.rectTransform.DOScale(1f, duration).SetEase(Ease.OutBack))
                .Join(_text.DOFade(1f, duration))
                .Join(_bubbleImage.DOFade(1f, duration))
                .AppendInterval(_showTime)
                .Append(_text.DOFade(0f, 0.4f))
                .Join(_bubbleImage.DOFade(0f, 0.4f))
                .Join(_bubbleImage.rectTransform.DOScale(0.8f, 0.4f).SetEase(Ease.InBack))
                .OnComplete(Hide);
        }

        public void ShowTextList(List<string> messages)
        {
            _messages = messages;
            _currentMessageIndex = 0;
            ShowText(_messages[_currentMessageIndex]);
        }

        private void ShowText(string message)
        {
            _text.text = message;
            _text.alpha = 0f;

            Show();
            _bubbleImage.color = new Color(_bubbleImage.color.r, _bubbleImage.color.g, _bubbleImage.color.b, 0f);

            Vector2 startPos = _bubbleImage.rectTransform.anchoredPosition;
            _bubbleImage.rectTransform.localScale = Vector3.zero;
            _bubbleImage.rectTransform.anchoredPosition = startPos;

            _showSequence?.Kill();
            _showSequence = DOTween.Sequence();

            var duration = .4f;
            _showSequence
                .Append(_bubbleImage.rectTransform.DOScale(1f, duration).SetEase(Ease.OutBack))
                .Join(_text.DOFade(1f, duration))
                .Join(_bubbleImage.DOFade(1f, duration))
                .OnComplete(ShowNextButton);
        }

        private void ShowNextButton() => _nextButton.gameObject.SetActive(true);
        private void HideNextButton() => _nextButton.gameObject.SetActive(false);

        private void HideText()
        {
            _showSequence?.Kill();

            Sequence hideSeq = DOTween.Sequence();
            hideSeq.Append(_text.DOFade(0f, 0.4f))
                .Join(_bubbleImage.DOFade(0f, 0.4f))
                .Join(_bubbleImage.rectTransform.DOScale(0.8f, 0.4f).SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    _currentMessageIndex++;
                    Hide();
                    HideNextButton();
                    if (HasNextMessage())
                        ShowText(_messages[_currentMessageIndex]);
                });
        }

        private bool HasNextMessage() => _currentMessageIndex <= _messages.Count;

        private void OnDestroy() => _nextButton.onClick.RemoveAllListeners();
    }
}