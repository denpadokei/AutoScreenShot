using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using SiraUtil.Converters;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace AutoScreenShot.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual bool Enable { get; set; } = true;
        public virtual bool NoUI { get; set; } = false;
        public virtual int MinSec { get; set; } = 6;
        public virtual int MaxSec { get; set; } = 120;
        public virtual int PositionScale { get; set; } = 10;
        public virtual float MinFoV { get; set; } = 15;
        public virtual float MaxFoV { get; set; } = 110;
        public float MenuPictuersMinRadius { get; set; } = 5f;
        public float MenuPictuersMaxRadius { get; set; } = 7f;
        public virtual bool ShowPictureInMenu { get; set; } = true;
        [UseConverter(typeof(EnumConverter<ImageExtention>))]
        public virtual ImageExtention Extention { get; set; } = ImageExtention.JPEG;
        public virtual int PictureCount { get; set; } = 500;
        [UseConverter(typeof(Vector3Converter))]
        public virtual Vector3 TargetOffset { get; set; } = Vector3.zero;
        [UseConverter(typeof(Vector2Converter))]
        public virtual Vector2 PictuerRenderSize { get; set; } = new Vector2(1920, 1080);
        public virtual int AntiAliasing { get; set; } = 1;

        public event Action<PluginConfig> OnConfigChanged;
        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed() =>
            // Do stuff when the config is changed.
            this.OnConfigChanged?.Invoke(this);

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}
