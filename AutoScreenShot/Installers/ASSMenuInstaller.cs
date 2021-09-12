using AutoScreenShot.Models;
using AutoScreenShot.Views;
using SiraUtil;
using Zenject;

namespace AutoScreenShot.Installers
{
    public class ASSMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<FloatingImageController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
