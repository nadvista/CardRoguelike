using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Desk;
using Core.Tools;
using Implementation.Actors;
using Implementation.Battle;
using Implementation.Desks;
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
        private Timer timer;

        [SerializeField]
        private ScoreCounter scoreCounter;

        public override void InstallBindings()
        {
            Container.Bind<ActorsDatabase>().FromInstance(actors).AsSingle();
            Container.Bind<DesksDatabase>().FromInstance(desks).AsSingle();

            Container.Bind<Timer>().FromInstance(timer).AsSingle();
            Container.Bind<ScoreCounter>().FromInstance(scoreCounter).AsSingle();

            Container.Bind<IPlayerProvider>().To<RandomPlayerProvider>().AsSingle();
            Container.Bind<IEnemyProvider>().To<ContiniousEnemiesProvider>().AsSingle();

            Container.Bind<IDesksProvider>().To<RandomDeskProvider>().AsSingle();
            Container.Bind<IBattleProvider>().To<BattleProvider>().AsSingle();
        }
    }
}
