using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class AutoScrollController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _followSpeed = 5f;
        [SerializeField] private float _scrollTimeout = 0.5f;
        [SerializeField] private float _epsilon = 0.001f;
        private RectTransform _target;
        private RectTransform _viewport;

        private bool _isUserScrolling;
        private float _lastScrollTime;
        private Vector2 _lastScrollPosition;
        private bool _userInterrupted;
        private Vector2 _lastUserPosition;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (_scrollRect != null)
            {
                _viewport = _scrollRect.viewport;

                if (_content == null && _scrollRect.content != null)
                    _content = _scrollRect.content;
            }
        }

        private void Update()
        {
            CheckUserScrollEnd();

            if (!_isUserScrolling && _target != null && _content != null && _viewport != null)
            {
                FollowTarget();
            }
        }

        public void SetTarget(RectTransform target)
        {
            _target = target;
            ResumeFollowing();
        }

        public void ResumeFollowing()
        {
            _isUserScrolling = false;
            _userInterrupted = false;
            _lastScrollTime = Time.time;
        }

        private void CheckUserScrollEnd()
        {
            if (_isUserScrolling && Time.time - _lastScrollTime > _scrollTimeout)
            {
                _isUserScrolling = false;
                _userInterrupted = true;
            }
        }

        private void FollowTarget()
        {
            if (_userInterrupted) return;

            var localTargetPos = _content.InverseTransformPoint(_target.position);
            var contentWidth = _content.rect.width - _viewport.rect.width;

            if (contentWidth <= 0) return;

            var targetNormalizedX = Mathf.Clamp01((localTargetPos.x - _viewport.rect.width / 2f) / contentWidth);

            _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
                _scrollRect.horizontalNormalizedPosition,
                targetNormalizedX,
                Time.deltaTime * _followSpeed
            );

            if (Mathf.Abs(_scrollRect.horizontalNormalizedPosition - targetNormalizedX) < _epsilon)
            {
                _scrollRect.horizontalNormalizedPosition = targetNormalizedX;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isUserScrolling = true;
            _userInterrupted = true;
            _lastScrollTime = Time.time;
            _lastScrollPosition = _scrollRect.normalizedPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _lastScrollTime = Time.time;
            _lastUserPosition = _scrollRect.normalizedPosition;
        }

        public void OnEndDrag(PointerEventData eventData) =>
            _lastScrollTime = Time.time;


#if UNITY_EDITOR
        private void OnValidate()
        {
            InitializeComponents();
        }
#endif
    }
}