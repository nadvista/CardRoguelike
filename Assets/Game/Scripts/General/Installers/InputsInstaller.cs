using Inputs.Battle;
using Inputs.SimpleHandlers;
using Zenject;

namespace General.Installers
{
    public class InputsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplaySimpleInputsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputsBattleController>().AsSingle();
        }
    }
}
