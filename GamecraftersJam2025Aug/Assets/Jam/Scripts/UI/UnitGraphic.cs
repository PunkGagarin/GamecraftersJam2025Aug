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

        private bool _isDead;

        [SerializeField]
        private Animator _animator;

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
            }
        }

        public async UniTask TakeDamage()
        {
            if (_isDead) return;
            try
            {
                _animator.SetTrigger(TakeDamageName);
                await WaitAnimation();
                Idle();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async UniTask Death()
        {
            _isDead = true;
            try
            {
                _animator.SetTrigger(DeathName);
                await WaitAnimation();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTask WaitAnimation()
        {
            await UniTask.Yield();

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;
            if (animLength > 2f)
                animLength = 2f;
            Debug.Log($" Animation {_animator.GetCurrentAnimatorStateInfo(0).nameHash} length {animLength}");

            await UniTask.Delay((int)(animLength * 1000));
        }
    }
}