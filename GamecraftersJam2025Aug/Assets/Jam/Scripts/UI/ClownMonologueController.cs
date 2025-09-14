using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.UI
{
    public class ClownMonologueController : ContentUi
    {
        private enum DisplayMode
        {
            Manual,
            Timed
        }

        public event Action OnTypingStarted;
        public event Action OnTypingEnded;
        public event Action OnDialogueCompleted = delegate { };

        [Inject] private AudioService _audioService;

        [SerializeField]
        private Image _bubbleImage;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private float _showTime;

        [SerializeField]
        private float _typewriterSpeed = 0.05f;

        private DisplayMode _currentMode = DisplayMode.Manual;
        private List<string> _messages = new();
        private int _currentMessageIndex;
        private Sequence _showSequence;
        private bool _isTyping;
        private CancellationTokenSource _autoHideCts;

        private void Awake() => _nextButton.onClick.AddListener(OnNextClicked);

        private void OnNextClicked()
        {
            if (_isTyping)
            {
                _isTyping = false;
                _text.maxVisibleCharacters = _text.text.Length;
                return;
            }

            if (_currentMode == DisplayMode.Timed)
            {
                _autoHideCts?.Cancel();
            }

            HideText();
        }

        public void ShowTextWithTimer(string message)
        {
            _nextButton.gameObject.SetActive(false);
            _currentMode = DisplayMode.Timed;
            _messages = new List<string>();
            _currentMessageIndex = 0;

            PrepareBubbleForShow();

            _showSequence?.Kill();
            _showSequence = DOTween.Sequence();

            var duration = .4f;
            _showSequence
                .Append(_bubbleImage.rectTransform.DOScale(1f, duration).SetEase(Ease.OutBack))
                .Join(_text.DOFade(1f, duration))
                .Join(_bubbleImage.DOFade(1f, duration))
                .OnComplete(() => { _ = OnShowCompleteForTimed(message); });
        }

        public void ShowTextList(List<string> messages)
        {
            _currentMode = DisplayMode.Manual;
            _messages = messages ?? new List<string>();
            _currentMessageIndex = 0;

            if (_messages.Count > 0)
                ShowText(_messages[_currentMessageIndex]);
        }

        private async UniTask OnShowCompleteForTimed(string message)
        {
            _autoHideCts?.Cancel();
            _autoHideCts?.Dispose();
            _autoHideCts = new CancellationTokenSource();

            await TypewriterEffect(message);

            try
            {
                await UniTask.Delay((int)(_showTime * 1000f), cancellationToken: _autoHideCts.Token);
                HideText();
            }
            catch (Exception e)
            {
                // таймер прерван кликом
            }
        }

        public void ShowText(string message)
        {
            _currentMode = DisplayMode.Manual;

            PrepareBubbleForShow();

            _showSequence?.Kill();
            _showSequence = DOTween.Sequence();

            var duration = .4f;
            _showSequence
                .Append(_bubbleImage.rectTransform.DOScale(1f, duration).SetEase(Ease.OutBack))
                .Join(_text.DOFade(1f, duration))
                .Join(_bubbleImage.DOFade(1f, duration))
                .OnComplete(() => { _ = TypewriterEffect(message); });

            _nextButton.gameObject.SetActive(true);
        }

        private void PrepareBubbleForShow()
        {
            _autoHideCts?.Cancel();
            _autoHideCts?.Dispose();
            _autoHideCts = null;

            _text.text = "";
            _text.alpha = 0f;
            _text.maxVisibleCharacters = 0;

            Show();
            _bubbleImage.color = new Color(_bubbleImage.color.r, _bubbleImage.color.g, _bubbleImage.color.b, 0f);

            Vector2 startPos = _bubbleImage.rectTransform.anchoredPosition;
            _bubbleImage.rectTransform.localScale = Vector3.zero;
            _bubbleImage.rectTransform.anchoredPosition = startPos;
        }

        private async UniTask TypewriterEffect(string message)
        {
            _isTyping = true;
            _text.text = message;
            _text.maxVisibleCharacters = 0;

            OnTypingStarted?.Invoke();
            TypingStarted();

            for (int i = 1; i <= message.Length; i++)
            {
                if (!_isTyping) break;
                _text.maxVisibleCharacters = i;
                await UniTask.Delay((int)(_typewriterSpeed * 1000f));
            }

            if (!_isTyping)
                _text.maxVisibleCharacters = message.Length;

            _isTyping = false;

            OnTypingEnded?.Invoke();
            TypingEnded();
        }

        protected virtual void TypingStarted()
        {
            _audioService.PlaySfxLoop(Sounds.clownTalking.ToString());
        }

        protected virtual void TypingEnded()
        {
            _audioService.StopSfxLoop();
        }

        private void HideText()
        {
            _autoHideCts?.Cancel();
            _autoHideCts?.Dispose();
            _autoHideCts = null;

            _showSequence?.Kill();

            Sequence hideSeq = DOTween.Sequence();
            hideSeq.Append(_text.DOFade(0f, 0.4f))
                .Join(_bubbleImage.DOFade(0f, 0.4f))
                .Join(_bubbleImage.rectTransform.DOScale(0.8f, 0.4f).SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    Hide();
                    if (_currentMode == DisplayMode.Manual)
                    {
                        _currentMessageIndex++;
                        if (HasNextMessage())
                        {
                            ShowText(_messages[_currentMessageIndex]);
                            return;
                        }
                        else
                        {
                            Debug.LogError(" OnDialogueCompleted invoke");
                            OnDialogueCompleted.Invoke();
                        }
                    }

                    _nextButton.gameObject.SetActive(false);
                });
        }

        private bool HasNextMessage()
        {
            return _currentMessageIndex < _messages.Count;
        }

        private void OnDestroy()
        {
            _nextButton.onClick.RemoveAllListeners();
            _showSequence?.Kill();
            _autoHideCts?.Cancel();
            _autoHideCts?.Dispose();
        }
    }
}