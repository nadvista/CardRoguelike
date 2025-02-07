using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Desk;
using Core.ScoreCounting;
using Core.Steps;
using Core.Tools;
using Core.Tools.Pool;
using Implementation.Actors;
using Implementation.Battle;
using Implementation.Desks;
using Implementation.Params.Modifiers;
using UnityEngine;
using Zenject;

namespace Implementation.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private ActorsDatabase actors;

        [SerializeField]
        private DesksDatabase desks;

        [SerializeField]
        private ScoreCounter scoreCounter;

        public override void InstallBindings()
        {
            InstallDatabases();

            Container.BindInterfacesAndSelfTo<Timer>().AsSingle();

            Container.BindInterfacesAndSelfTo<ScoreCounter>().FromInstance(scoreCounter).AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalStepsCounter>().AsSingle();

            InstallProviders();

            InstallModifierFabrics();
        }

        private void InstallModifierFabrics()
        {
            Container.BindInterfacesAndSelfTo<SubtractModifierFabric>().AsSingle();

            Container.Bind<ModifiersPool>().AsSingle();
        }

        private void InstallDatabases()
        {
            Container.Bind<ActorsDatabase>().FromInstance(actors).AsSingle();
            Container.Bind<DesksDatabase>().FromInstance(desks).AsSingle();
        }

        private void InstallProviders()
        {
            Container.BindInterfacesAndSelfTo<RandomPlayerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ContiniousEnemiesProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomDeskProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleProvider>().AsSingle();
        }
    }
}
