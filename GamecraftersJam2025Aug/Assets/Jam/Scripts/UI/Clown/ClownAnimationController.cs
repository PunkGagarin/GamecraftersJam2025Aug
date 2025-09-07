using UnityEngine;

namespace Jam.Scripts.UI.Clown
{
    public class ClownAnimationController : MonoBehaviour
    {
        private static readonly int Idle1 = Animator.StringToHash("Idle1");
        private static readonly int Idle2 = Animator.StringToHash("Idle2");
        private static readonly int TalkAngry = Animator.StringToHash("TalkAngry");
        private static readonly int TalkSmile = Animator.StringToHash("TalkSmile");
        private static readonly int Smile = Animator.StringToHash("Smile");
        private static readonly int Sad = Animator.StringToHash("Sad");
        private static readonly int Nod = Animator.StringToHash("Nod");
        private static readonly int IdleInstant = Animator.StringToHash("IdleInstant");

        [SerializeField] private Animator _animator;

        public void PlayIdle()
        {
            _animator.SetTrigger(IdleInstant);
            Debug.Log("SetTrigger(IdleInstant)");
        }

        public void PlayIdle1() => _animator.SetTrigger(Idle1);
        public void PlayIdle2() => _animator.SetTrigger(Idle2);
        public void PlayTalkAngry()
        {
            _animator.SetTrigger(TalkAngry);
            Debug.Log("SetTrigger(IdleInstant)");
        }

        public void PlayTalkSmile()
        {
            _animator.SetTrigger(TalkSmile);
            Debug.Log("SetTrigger(TalkSmile)");
        }

        public void PlaySmile()
        {
            _animator.SetTrigger(Smile);
            Debug.Log("SetTrigger(Smile)");
        }

        public void PlaySad()
        {
            _animator.SetTrigger(Sad);
            Debug.Log("SetTrigger(Sad)");
        }

        public void PlayNod() => _animator.SetTrigger(Nod);
    }
}