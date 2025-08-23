using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Configs Installer", menuName = "Gameplay/Configs/ConfigInstaller")]
    public class GameplayConfigsInstaller : ScriptableObjectInstaller
    {

        [SerializeField]
        private BallsConfigRepository _ballsConfigRepository;
        
        [SerializeField]
        private EnemyConfigRepository _enemyConfigRepository;

        public override void InstallBindings()
        {
            Container.Bind<BallsConfigRepository>().FromInstance(_ballsConfigRepository).AsSingle();
            Container.Bind<EnemyConfigRepository>().FromInstance(_enemyConfigRepository).AsSingle();
        }

    }
}