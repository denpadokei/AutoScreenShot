using SiraUtil;
using Zenject;

namespace AutoScreenShot.Installers
{
    public class ASSGameInstaller : MonoInstaller
    {
        public override void InstallBindings() => this.Container.BindInterfacesAndSelfTo<AutoScreenShotController>().FromNewComponentOnNewGameObject(nameof(AutoScreenShotController)).AsCached();
    }
}
