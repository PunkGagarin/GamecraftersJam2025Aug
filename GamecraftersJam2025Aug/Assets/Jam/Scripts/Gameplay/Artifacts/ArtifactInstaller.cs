using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactInstaller : MonoInstaller
    {
        [field: SerializeField]
        private ArtifactTestView View { get; set; }

        [field: SerializeField]
        private ArtifactSoRepository Repo { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ArtifactTestView>().FromInstance(View).AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactSoRepository>().FromInstance(Repo).AsSingle();

            Container.BindInterfacesAndSelfTo<ArtifactBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactModelFactory>().AsSingle();

            BindFactories();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<ArtifactFactoryRegistry>().AsSingle();

            Container
                .BindFactory<ArtifactSo, ArtifactHealFromDamageSystem, ArtifactHealFromDamageSystem.ArtifactFactory>()
                .AsSingle();

            Container.BindFactory<ArtifactSo, ArtifactHealIncreaseSystem, ArtifactHealIncreaseSystem.ArtifactFactory>()
                .AsSingle();

            Container.BindFactory<ArtifactSo, ArtifactHealOnCritSystem, ArtifactHealOnCritSystem.ArtifactFactory>()
                .AsSingle();

            Container
                .BindFactory<ArtifactSo, ArtifactHealOnShuffleSystem, ArtifactHealOnShuffleSystem.ArtifactFactory>()
                .AsSingle();

            Container
                .BindFactory<ArtifactSo, ArtifactMaxHpEndBattleIncreaseSystem,
                    ArtifactMaxHpEndBattleIncreaseSystem.ArtifactFactory>()
                .AsSingle();

            Container
                .BindFactory<ArtifactSo, ArtifactDamageIncreaseSystem, ArtifactDamageIncreaseSystem.ArtifactFactory>()
                .AsSingle();
            
            Container
                .BindFactory<ArtifactSo, ArtifactDamageAfterKillIncreaseSystem, ArtifactDamageAfterKillIncreaseSystem.ArtifactFactory>()
                .AsSingle();            
            Container
                .BindFactory<ArtifactSo, ArtifactDamageAllOnRoundStartSystem, ArtifactDamageAllOnRoundStartSystem.ArtifactFactory>()
                .AsSingle();         
            Container
                .BindFactory<ArtifactSo, ArtifactDamageAfterQueueShuffleIncreaseSystem, ArtifactDamageAfterQueueShuffleIncreaseSystem.ArtifactFactory>()
                .AsSingle();         
            Container
                .BindFactory<ArtifactSo, ArtifactDamageFromHealSystem, ArtifactDamageFromHealSystem.ArtifactFactory>()
                .AsSingle();
        }
    }
}