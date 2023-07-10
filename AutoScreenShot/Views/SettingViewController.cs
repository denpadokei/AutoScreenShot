using AutoScreenShot.Configuration;
using AutoScreenShot.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Zenject;

namespace AutoScreenShot.Views
{
    [HotReload]
    public class SettingViewController : BSMLAutomaticViewController, IInitializable
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", this.GetType().Namespace, this.GetType().Name);

        public void Initialize()
        {
            this.Enable = PluginConfig.Instance.Enable;
            this.NoUI = PluginConfig.Instance.NoUI;
            this.IncludeHMDObject = PluginConfig.Instance.IncludeHMDOnlyObject;
            this.ShowInMenu = PluginConfig.Instance.ShowPictureInMenu;
            this.MenuOverlap = PluginConfig.Instance.MenuPictureOverlap;
            this.MinFov = PluginConfig.Instance.MinFoV;
            this.MaxFov = PluginConfig.Instance.MaxFoV;
            this.PictureCount = PluginConfig.Instance.PictureCount;
            BSMLSettings.instance.AddSettingsMenu("AutoScreenShot", this.ResourceName, this);
        }
        private void OnDisable()
        {
            PluginConfig.Instance.OnConfigChanged -= this.OnConfigChanged;
        }

        private void OnEnable()
        {
            PluginConfig.Instance.OnConfigChanged += this.OnConfigChanged;
        }

        private void OnConfigChanged(PluginConfig obj)
        {
            this.Enable = obj.Enable;
            this.NoUI = obj.NoUI;
            this.IncludeHMDObject = obj.IncludeHMDOnlyObject;
            this.ShowInMenu = obj.ShowPictureInMenu;
            this.MenuOverlap = obj.MenuPictureOverlap;
            this.MinFov = obj.MinFoV;
            this.MaxFov = obj.MaxFoV;
            this.PictureCount = obj.PictureCount;
        }

        private bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string memberName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) {
                return false;
            }

            field = newValue;
            MainThreadInvoker.Instance.Enqueue(() => this.OnPropertyChanged(memberName));
            return true;
        }

        protected void OnPropertyChanged(string e)
        {
            this.NotifyPropertyChanged(e);
            if (e == nameof(this.Enable)) {
                PluginConfig.Instance.Enable = this.Enable;
            }
            else if (e == nameof(this.NoUI)) {
                PluginConfig.Instance.NoUI = this.NoUI;
            }
            else if (e == nameof(this.IncludeHMDObject)) {
                PluginConfig.Instance.IncludeHMDOnlyObject = this.IncludeHMDObject;
            }
            else if (e == nameof(this.ShowInMenu)) {
                PluginConfig.Instance.ShowPictureInMenu = this.ShowInMenu;
            }
            else if (e == nameof(this.MenuOverlap)) {
                PluginConfig.Instance.MenuPictureOverlap = this.MenuOverlap;
            }
            else if (e == nameof(this.MinFov)) {
                PluginConfig.Instance.MinFoV = this.MinFov;
            }
            else if (e == nameof(this.MaxFov)) {
                PluginConfig.Instance.MaxFoV = this.MaxFov;
            }
            else if (e == nameof(this.PictureCount)) {
                PluginConfig.Instance.PictureCount = this.PictureCount;
            }
        }

        [UIAction("clear")]
        public void ClearChache()
        {
            FloatingImageCanvas.ClearChache();
        }

        /// <summary>有効かどうか を取得、設定</summary>
        private bool _enable;
        [UIValue("enable")]
        /// <summary>有効かどうか を取得、設定</summary>
        public bool Enable
        {
            get => this._enable;

            set => this.SetProperty(ref this._enable, value);
        }


        /// <summary>UIを非表示にするか を取得、設定</summary>
        private bool _noUI;
        [UIValue("no-ui")]
        /// <summary>UIを非表示にするか を取得、設定</summary>
        public bool NoUI
        {
            get => this._noUI;

            set => this.SetProperty(ref this._noUI, value);
        }

        /// <summary>HMDのみ映るGameObjectを含むかどうか を取得、設定</summary>
        private bool _includeHMDObject;
        [UIValue("include-hmd-only-object")]
        /// <summary>HMDのみ映るGameObjectを含むかどうか を取得、設定</summary>
        public bool IncludeHMDObject
        {
            get => this._includeHMDObject;

            set => this.SetProperty(ref this._includeHMDObject, value);
        }


        /// <summary>メニューで表示するかどうか を取得、設定</summary>
        private bool _showInMenu;
        [UIValue("show-in-menu")]
        /// <summary>メニューで表示するかどうか を取得、設定</summary>
        public bool ShowInMenu
        {
            get => this._showInMenu;

            set => this.SetProperty(ref this._showInMenu, value);
        }

        /// <summary>メニューに重ねて表示するかどうか を取得、設定</summary>
        private bool _menuOverlap;
        [UIValue("menu-overlap")]
        /// <summary>メニューに重ねて表示するかどうか を取得、設定</summary>
        public bool MenuOverlap
        {
            get => this._menuOverlap;

            set => this.SetProperty(ref this._menuOverlap, value);
        }

        /// <summary>最小値のFOV を取得、設定</summary>
        private float _minFov;
        [UIValue("min-fov")]
        /// <summary>最小値のFOV を取得、設定</summary>
        public float MinFov
        {
            get => this._minFov;

            set => this.SetProperty(ref this._minFov, value);
        }

        /// <summary>最大のFOV を取得、設定</summary>
        private float _maxFov;
        [UIValue("max-fov")]
        /// <summary>最大のFOV を取得、設定</summary>
        public float MaxFov
        {
            get => this._maxFov;

            set => this.SetProperty(ref this._maxFov, value);
        }

        /// <summary>写真の数 を取得、設定</summary>
        private int _pictureCount;
        [UIValue("pictuer-count")]
        /// <summary>写真の数 を取得、設定</summary>
        public int PictureCount
        {
            get => this._pictureCount;

            set => this.SetProperty(ref this._pictureCount, value);
        }
    }
}
