using Core.ScoreCounting;
using Core.Steps;
using Core.Tools.Timer;
using Implementation.Actors;
using Implementation.Battle;
using Implementation.Cards;
using Implementation.Desks;
using Implementation.Params.Modifiers;
using UnityEngine;
using Zenject;

namespace General.Installers
{

    public class BattleSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private ScoreCounter scoreCounter;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreCounter>().FromInstance(scoreCounter).AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalStepsCounter>().AsSingle();

            InstallModifierFabrics();

            InstallProviders();
        }

        private void InstallModifierFabrics()
        {
            Container.BindInterfacesAndSelfTo<ModifierFabric>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimersFabric>().AsSingle();

            Container.BindInterfacesAndSelfTo<TimersPool>().AsSingle();
            Container.Bind<ModifiersPool>().AsSingle();
        }

        private void InstallProviders()
        {
            Container.BindInterfacesAndSelfTo<RandomPlayerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ContiniousEnemiesProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomDeskProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<CardsCooldownProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleProvider>().AsSingle();
        }
    }
}
