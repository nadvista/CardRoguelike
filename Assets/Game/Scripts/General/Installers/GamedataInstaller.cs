using Implementation.Actors;
using Implementation.Desks;
using UnityEngine;
using Zenject;

namespace General.Installers
{
    public class GamedataInstaller : MonoInstaller
    {
        [SerializeField]
        private ActorsDatabase actors;

        [SerializeField]
        private DesksDatabase desks;

        public override void InstallBindings()
        {
            Container.Bind<ActorsDatabase>().FromInstance(actors).AsSingle();
            Container.Bind<DesksDatabase>().FromInstance(desks).AsSingle();
        }
    }
}
