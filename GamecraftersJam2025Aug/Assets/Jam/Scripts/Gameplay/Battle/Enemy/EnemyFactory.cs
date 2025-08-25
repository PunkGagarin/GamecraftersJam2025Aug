using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyFactory
    {
        [Inject] private EnemyBusEvent _enemyBus;
        [Inject] private EnemyConfigRepository _enemyConfigRepository;

        public EnemyModel CreateEnemy()
        {
            EnemySo enemyConfig = _enemyConfigRepository.GetRandomEnemy();
            return new EnemyModel(enemyConfig.Damage, enemyConfig.Health, enemyConfig.Type, _enemyBus);
        }
    }
}