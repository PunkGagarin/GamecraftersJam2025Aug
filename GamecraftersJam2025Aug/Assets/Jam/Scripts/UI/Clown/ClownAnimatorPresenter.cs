using System;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.UI.Clown
{
    public class ClownAnimatorPresenter : IInitializable, IDisposable
    {
        [Inject] private ClownEventBus _eventBus;
        [Inject] private ClownAnimationController _animationController;

        public void Initialize()
        {
            _eventBus.OnClownMonologueStart += OnClownMonologueStart;
            _eventBus.OnClownMonologueEnd += OnClownMonologueEnd;
            _eventBus.OnUserChoseCupSuccess += OnUserChoseCupSuccess;
            _eventBus.OnUserChoseCupFail += OnUserChoseCupFail;
        }

        private void OnUserChoseCupFail()
        {
            if (Random.value < 0.5f) 
                _animationController.PlaySmile();
            _animationController.PlayIdle1();
        }

        private void OnUserChoseCupSuccess()
        {
            if (Random.value < 0.5f)
                _animationController.PlaySad();
            _animationController.PlayIdle1();
        }

        private void OnClownMonologueEnd()
        {
            _animationController.PlayIdle1();
        }

        private void OnClownMonologueStart()
        {
            _animationController.PlayTalkSmile();
        }

        public void Dispose()
        {
            _eventBus.OnClownMonologueStart -= OnClownMonologueStart;
            _eventBus.OnClownMonologueEnd -= OnClownMonologueEnd;
            _eventBus.OnUserChoseCupSuccess -= OnUserChoseCupSuccess;
            _eventBus.OnUserChoseCupFail -= OnUserChoseCupFail;
        }
    }
}