using Core.ScoreCounting;
using Core.Steps;
using Core.Tools.Timer;
using Implementation.Actors;
using Implementation.Battle;
using Implementation.Cards;
using Implementation.Desks;
using Implementation.Params.Modifiers;
using Inputs.Battle;
using Inputs.SimpleHandlers;
using UI;
using UnityEngine;
using Zenject;

namespace General.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private ActorsDatabase actors;

        [SerializeField]
        private DesksDatabase desks;

        [SerializeField]
        private ScoreCounter scoreCounter;

        [SerializeField]
        private ParamsPreviews paramsPreview;

        public override void InstallBindings()
        {
            InstallDatabases();

            Container.BindInterfacesAndSelfTo<ScoreCounter>().FromInstance(scoreCounter).AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalStepsCounter>().AsSingle();

            InstallModifierFabrics();

            InstallProviders();

            InstallInputs();

            InstallUi();
        }

        private void InstallModifierFabrics()
        {
            Container.BindInterfacesAndSelfTo<SubtractModifierFabric>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimersFabric>().AsSingle();

            Container.BindInterfacesAndSelfTo<TimersPool>().AsSingle();
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

            Container.BindInterfacesAndSelfTo<CardsCooldownProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleProvider>().AsSingle();
        }
    
        private void InstallInputs()
        {
            Container.BindInterfacesAndSelfTo<GameplaySimpleInputsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputsBattleController>().AsSingle();
        }

        private void InstallUi()
        {
            Container.Bind<ParamsPreviews>().FromInstance(paramsPreview).AsSingle();
        }
    }
}
