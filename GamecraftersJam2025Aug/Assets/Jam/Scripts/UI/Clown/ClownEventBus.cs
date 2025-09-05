using System;

namespace Jam.Scripts.UI.Clown
{
    public class ClownEventBus
    {
        public event Action OnUserChoseCupSuccess = delegate { };
        public event Action OnUserChoseCupFail = delegate { };

        public event Action OnClownMonologueStart = delegate { };
        public event Action OnClownMonologueEnd = delegate { };


        public virtual void UserChoseCupSuccess() => OnUserChoseCupSuccess.Invoke();
        public virtual void UserChoseCupFail() => OnUserChoseCupFail.Invoke();
        public virtual void ClownMonologueStart() => OnClownMonologueStart.Invoke();
        public virtual void ClownMonologueEnd() => OnClownMonologueEnd.Invoke();
    }
}