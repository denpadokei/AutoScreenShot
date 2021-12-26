using AutoScreenShot.Configuration;
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
            this.ShowInMenu = PluginConfig.Instance.ShowPictureInMenu;
            this.MinFov = PluginConfig.Instance.MinFoV;
            this.MaxFov = PluginConfig.Instance.MaxFoV;
            this.PictureCount = PluginConfig.Instance.PictureCount;
            BSMLSettings.instance.AddSettingsMenu("AutoScreenShot", this.ResourceName, this);
        }
        private void OnDisable() => PluginConfig.Instance.OnConfigChanged -= this.OnConfigChanged;

        private void OnEnable() => PluginConfig.Instance.OnConfigChanged += this.OnConfigChanged;

        private void OnConfigChanged(PluginConfig obj)
        {
            this.Enable = obj.Enable;
            this.NoUI = obj.NoUI;
            this.ShowInMenu = obj.ShowPictureInMenu;
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
            HMMainThreadDispatcher.instance.Enqueue(() => this.OnPropertyChanged(memberName));
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
            else if (e == nameof(this.ShowInMenu)) {
                PluginConfig.Instance.ShowPictureInMenu = this.ShowInMenu;
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


        /// <summary>有効かどうか を取得、設定</summary>
        private bool enable_;
        [UIValue("enable")]
        /// <summary>有効かどうか を取得、設定</summary>
        public bool Enable
        {
            get => this.enable_;

            set => this.SetProperty(ref this.enable_, value);
        }


        /// <summary>UIを非表示にするか を取得、設定</summary>
        private bool noUI_;
        [UIValue("no-ui")]
        /// <summary>UIを非表示にするか を取得、設定</summary>
        public bool NoUI
        {
            get => this.noUI_;

            set => this.SetProperty(ref this.noUI_, value);
        }


        /// <summary>メニューで表示するかどうか を取得、設定</summary>
        private bool showInMenu_;
        [UIValue("show-in-menu")]
        /// <summary>メニューで表示するかどうか を取得、設定</summary>
        public bool ShowInMenu
        {
            get => this.showInMenu_;

            set => this.SetProperty(ref this.showInMenu_, value);
        }

        /// <summary>最小値のFOV を取得、設定</summary>
        private float minFov_;
        [UIValue("min-fov")]
        /// <summary>最小値のFOV を取得、設定</summary>
        public float MinFov
        {
            get => this.minFov_;

            set => this.SetProperty(ref this.minFov_, value);
        }

        /// <summary>最大のFOV を取得、設定</summary>
        private float maxFov_;
        [UIValue("max-fov")]
        /// <summary>最大のFOV を取得、設定</summary>
        public float MaxFov
        {
            get => this.maxFov_;

            set => this.SetProperty(ref this.maxFov_, value);
        }

        /// <summary>写真の数 を取得、設定</summary>
        private int pictureCount_;
        [UIValue("pictuer-count")]
        /// <summary>写真の数 を取得、設定</summary>
        public int PictureCount
        {
            get => this.pictureCount_;

            set => this.SetProperty(ref this.pictureCount_, value);
        }
    }
}
