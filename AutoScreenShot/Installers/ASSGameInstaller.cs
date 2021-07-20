using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using SiraUtil;

namespace AutoScreenShot.Installers
{
    public class ASSGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<AutoScreenShotController>().FromNewComponentOnNewGameObject(nameof(AutoScreenShotController)).AsCached();
        }
    }
}
