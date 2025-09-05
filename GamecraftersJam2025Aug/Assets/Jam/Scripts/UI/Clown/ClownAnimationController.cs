using UnityEngine;

namespace Jam.Scripts.UI
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

        [SerializeField] private Animator _animator;

        public void PlayIdle1() => _animator.SetTrigger(Idle1);
        public void PlayIdle2() => _animator.SetTrigger(Idle2);
        public void PlayTalkAngry() => _animator.SetTrigger(TalkAngry);
        public void PlayTalkSmile() => _animator.SetTrigger(TalkSmile);
        public void PlaySmile() => _animator.SetTrigger(Smile);
        public void PlaySad() => _animator.SetTrigger(Sad);
        public void PlayNod() => _animator.SetTrigger(Nod);
    }
}