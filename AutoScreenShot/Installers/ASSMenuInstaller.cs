using AutoScreenShot.Models;
using AutoScreenShot.Views;
using HMUI;
using UnityEngine;
using Zenject;

namespace AutoScreenShot.Installers
{
    public class ASSMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<FloatingImageController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
            this.Container.BindMemoryPool<FloatingImageCanvas, FloatingImageCanvas.Pool>().WithInitialSize(500).FromComponentInNewPrefab(this._screen).AsCached();
        }

        private readonly GameObject _screen = new GameObject(nameof(FloatingImageCanvas), typeof(RectTransform), typeof(Canvas), typeof(CurvedCanvasSettings), typeof(FloatingImageCanvas));
    }
}
