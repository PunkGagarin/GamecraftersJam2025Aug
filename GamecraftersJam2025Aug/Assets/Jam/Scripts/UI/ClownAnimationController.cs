using UnityEngine;
using AnimationClip = UnityEngine.AnimationClip;

namespace Jam.Scripts.UI
{
    public class ClownAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [Header("Animation clips")] 
        [SerializeField] private AnimationClip longIdleAnimationClip;
        [SerializeField] private AnimationClip shortWithRedEyesIdleAnimationClip;
        [SerializeField] private AnimationClip shortIdleAnimationClip;
        [SerializeField] private AnimationClip sadAnimationClip;
        [SerializeField] private AnimationClip smileAnimationClip;
        [SerializeField] private AnimationClip talk1AnimationClip;
        [SerializeField] private AnimationClip talk2AnimationClip;

        public void PlayLongIdle()
        {
            if (_animator != null && longIdleAnimationClip != null)
                _animator.Play(longIdleAnimationClip.name);
        }

        public void PlayShortIdleWithRedEyes()
        {
            if (_animator != null && shortWithRedEyesIdleAnimationClip != null)
                _animator.Play(shortWithRedEyesIdleAnimationClip.name);
        }

        public void PlayShortIdle()
        {
            if (_animator != null && shortIdleAnimationClip != null)
                _animator.Play(shortIdleAnimationClip.name);
        }

        public void PlaySad()
        {
            if (_animator != null && sadAnimationClip != null)
                _animator.Play(sadAnimationClip.name);
        }

        public void PlaySmile()
        {
            if (_animator != null && smileAnimationClip != null)
                _animator.Play(smileAnimationClip.name);
        }

        public void PlayTalk1()
        {
            if (_animator != null && talk1AnimationClip != null)
                _animator.Play(talk1AnimationClip.name);
        }

        public void PlayTalk2()
        {
            if (_animator != null && talk2AnimationClip != null)
                _animator.Play(talk2AnimationClip.name);
        }
    }
}