using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jam.Scripts.UI
{
    public class UnitGraphic : MonoBehaviour, IUnitAniamtion
    {
        private static readonly int IdleName = Animator.StringToHash("Idle");
        private static readonly int AttackName = Animator.StringToHash("Attack");
        private static readonly int TakeDamageName = Animator.StringToHash("TakeDamage");
        private static readonly int DeathName = Animator.StringToHash("Death");

        [FormerlySerializedAs("animator")] [SerializeField] private Animator _animator;
        
        public async Task Idle() => _animator.SetTrigger(IdleName);

        public async Task Attack()
        {
            _animator.SetTrigger(AttackName);

            // Ждём, пока Animator реально переключится в нужный стейт
            await Task.Yield(); // пропускаем 1 кадр

            // Теперь получаем информацию о текущем стейте
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;

            // Ждём длительность анимации
            await Task.Delay((int)(animLength * 1000));
        }

        public async Task TakeDamage() => _animator.SetTrigger(TakeDamageName);

        public async Task Death() => _animator.SetTrigger(DeathName);
    }
}