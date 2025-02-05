using Core.Actors;
using Core.Battle;
using Core.Desk;
using Core.Tools;
using UnityEngine;
using Zenject;

namespace Core.Installers
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
            Container.Bind<ActorsProvider>().FromNew().AsSingle();
            Container.Bind<DeskProvider>().FromNew().AsSingle();
            Container.Bind<BattleProvider>().FromNew().AsSingle();
        }
    }
}
