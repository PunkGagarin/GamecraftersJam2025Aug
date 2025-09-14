using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jam.Scripts.UI
{
    public class UnitGraphic : MonoBehaviour
    {
        private static readonly int IdleName = Animator.StringToHash("Idle");
        private static readonly int AttackName = Animator.StringToHash("Attack");
        private static readonly int TakeDamageName = Animator.StringToHash("TakeDamage");
        private static readonly int DeathName = Animator.StringToHash("Death");

        [SerializeField] private Animator _animator;

        public void Idle() => _animator.SetTrigger(IdleName);

        public async UniTask Attack()
        {
            try
            {
                _animator.SetTrigger(AttackName);
                await WaitAnimation();
                Idle();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                // await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public async UniTask TakeDamage()
        {
            try
            {
                _animator.SetTrigger(TakeDamageName);
                await WaitAnimation();
                Idle();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                // await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public async UniTask Death()
        {
            try
            {
                _animator.SetTrigger(DeathName);
                await WaitAnimation();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                // await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async UniTask WaitAnimation()
        {
            Debug.Log($" Wait animation {_animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
            await UniTask.Delay(100);

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;
            Debug.Log($" Animation {_animator.GetCurrentAnimatorStateInfo(0).nameHash} length {animLength}");

            await UniTask.Delay((int)(animLength * 1000));
        }
    }
}