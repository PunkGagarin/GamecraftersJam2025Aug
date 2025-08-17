using System;
using Jam.Scripts.Utils.Pause;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Utils.Popup
{
    public abstract class Popup : MonoBehaviour
    {
        [Inject] private PauseService _pauseService;
        
        private bool _openedWithPause;
        
        private event Action OnClose;
        private event Action OnOpen;
        
        public void SetCloseEvent(Action onClose) => 
            OnClose = onClose;
        
        public void SetOpenEvent(Action onOpen) => 
            OnOpen = onOpen;
        
        public virtual void Open(bool withPause)
        {
            _openedWithPause = withPause;
            OnClose = null;
            
            if (withPause)
                _pauseService.SetPaused(true);
            
            gameObject.SetActive(true);
            OnOpen?.Invoke();
        }

        public virtual void Close()
        {
            if (_openedWithPause)
                _pauseService.SetPaused(false);

            gameObject.SetActive(false);
            OnClose?.Invoke();
        }
    }
}
