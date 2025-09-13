using UnityEngine;

namespace Jam.Scripts.UI
{
    public class UnitGraphic : MonoBehaviour, IUnitAniamtion
    {
        private static readonly int IdleName = Animator.StringToHash("Idle");
        private static readonly int AttackName = Animator.StringToHash("Attack");
        private static readonly int TakeDamageName = Animator.StringToHash("TakeDamage");
        private static readonly int DeathName = Animator.StringToHash("Death");

        [SerializeField] private Animator animator;
        
        public void Idle() => animator.SetTrigger(IdleName);

        public void Attack() => animator.SetTrigger(AttackName);

        public void TakeDamage() => animator.SetTrigger(TakeDamageName);

        public void Death() => animator.SetTrigger(DeathName);
    }
}