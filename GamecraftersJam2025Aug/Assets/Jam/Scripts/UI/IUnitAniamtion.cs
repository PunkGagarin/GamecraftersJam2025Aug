using System.Threading.Tasks;

namespace Jam.Scripts.UI
{
    public interface IUnitAniamtion
    {
        Task Idle();
        Task Attack();
        Task  TakeDamage();
        Task  Death();
    }
}