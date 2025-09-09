using Jam.Scripts.Gameplay.Artifacts.Data;
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
            Container.BindInterfacesAndSelfTo<ArtifactFactoryRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArtifactModelFactory>().AsSingle();

            BindFactories();
        }

        private void BindFactories()
        {
            Container.BindFactory<ArtifactShuffleReloadHealSystem, ZenjectArtifactShuffleHealFactory>()
                .FromNew();
            Container.BindInterfacesAndSelfTo<ArtifactShuffleHealFactory>().AsSingle();


            Container.BindFactory<ArtifactHealOnCritSystem, ZenjectArtifactHealOnCritFactory>()
                .FromNew();
            Container.BindInterfacesAndSelfTo<ArtifactHealOnCritFactory>().AsSingle();

            Container.BindFactory<ArtifactHealIncreaseSystem, ZenjectArtifactHealIncreaseFactory>()
                .FromNew();
            Container.BindInterfacesAndSelfTo<ArtifactHealIncreaseFactory>().AsSingle();

            Container.BindFactory<ArtifactHealFromDamageSystem, ZenjectArtifactHealFromDamageFactory>()
                .FromNew();
            Container.BindInterfacesAndSelfTo<ArtifactHealFromDamageFactory>().AsSingle();
        }
    }
}