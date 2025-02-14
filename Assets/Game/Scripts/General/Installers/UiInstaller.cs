using Ui.Hint;
using UI;
using UnityEngine;
using Zenject;

namespace General.Installers
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField]
        private ParamsDatas paramsPreview;

        [SerializeField]
        private HintView hintView;

        public override void InstallBindings()
        {
            Container.Bind<ParamsDatas>().FromInstance(paramsPreview).AsSingle();
            Container.Bind<HintView>().FromInstance(hintView).AsSingle();
        }
    }
}
