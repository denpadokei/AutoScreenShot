using AutoScreenShot.Configuration;
using AutoScreenShot.Extention;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Zenject;

namespace AutoScreenShot
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class AutoScreenShotController : MonoBehaviour, IInitializable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void Initialize()
        {
            if (!SystemInfo.supportsAsyncGPUReadback) {
                this._isSupprot = false;
                Plugin.Log.Debug("Not supproted.");
                return;
            }
            this._isSupprot = true;
            if (!Directory.Exists(_dataDir)) {
                Directory.CreateDirectory(_dataDir);
            }
            this.width = (int)PluginConfig.Instance.PictuerRenderSize.x;
            this.height = (int)PluginConfig.Instance.PictuerRenderSize.y;
            this._nextShootTime = DateTime.Now.AddSeconds(this._random.Next(this.minsec, this.maxsec));
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void Shoot()
        {
            if (!this._isSupprot) {
                return;
            }
            this.SetCameraPos(this.CreateCameraPos());
            var texture = RenderTexture.GetTemporary(this.width, this.height, 24, RenderTextureFormat.ARGB32);
            var oldtextuer = this._ssCamera.targetTexture;
            this._ssCamera.targetTexture = texture;
            this._ssCamera.Render();
            this._ssCamera.targetTexture = oldtextuer;
            AsyncGPUReadback.Request(texture, 0, req =>
            {
                if (req.hasError) {
                    return;
                }
                var format = texture.graphicsFormat;
                using (var data = req.GetData<byte>()) {
                    var dataArray = data.ToArray();
                    _ = Task.Run(() =>
                    {
                        var pngBytes = ImageConversion.EncodeArrayToJPG(dataArray, format, (uint)this.width, (uint)this.height);
                        if (!Directory.Exists(_dataDir)) {
                            Directory.CreateDirectory(_dataDir);
                        }
                        File.WriteAllBytes(Path.Combine(_dataDir, $"BeatSaber_{DateTime.Now:yyyy_MM_dd_hh_mm_ss}.jpg"), pngBytes);
                    });
                }
            });
        }

        private void SetCameraPos(Vector3 pos)
        {
            this._ssCamera.transform.position = pos;
            this._ssCamera.fieldOfView = this._random.NextFloat(this.minFoV, this.maxFoV);
            this._ssCamera.transform.LookAt(this._targetGO.transform);
        }

        private Vector3 CreateCameraPos()
        {
            return new Vector3(
                this._random.NextFloat(-this._posScale * 0.5f, this._posScale * 0.5f),
                this._random.NextFloat(0, this._posScale * 0.5f),
                this._random.NextFloat(-this._posScale * 0.5f, this._posScale * 0.5f));
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private Camera _ssCamera;
        private GameObject _targetGO;
        private bool _isSupprot;
        private static readonly string _dataDir = Path.Combine(Environment.CurrentDirectory, "UserData", "ScreenShoots", $"{DateTime.Now:yyyy_MM_dd}");
        private DateTime _nextShootTime;
        private int width;
        private int height;
        private System.Random _random;
        private int minsec;
        private int maxsec;
        private float minFoV;
        private float maxFoV;
        private float _posScale;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        private void Constractor(IAudioTimeSource controller)
        {
            this._random = new System.Random(Environment.TickCount);

            this.minsec = controller.songEndTime < PluginConfig.Instance.MinSec ? (int)(controller.songEndTime / 2) : PluginConfig.Instance.MinSec;
            this.maxsec = controller.songEndTime < PluginConfig.Instance.MaxSec || PluginConfig.Instance.MinSec < this.minsec ? (int)controller.songEndTime : PluginConfig.Instance.MaxSec;

            this.minFoV = PluginConfig.Instance.MinFoV;
            this.maxFoV = PluginConfig.Instance.MinFoV < this.minFoV ? this.minFoV : PluginConfig.Instance.MaxFoV;

            this._posScale = PluginConfig.Instance.PositionScale;

            this._ssCamera = Instantiate(Camera.main.gameObject).GetComponent<Camera>();
            this._ssCamera.gameObject.name = "Prompt";
            this._ssCamera.gameObject.tag = "Untagged";
            this._ssCamera.name = "Prompt Camera";
            this._ssCamera.stereoTargetEye = StereoTargetEyeMask.None;
            this._ssCamera.gameObject.transform.position = new Vector3(0f, 1.7f, -3.2f);
            this._ssCamera.cullingMask = -1;

            this._ssCamera.targetTexture = new RenderTexture(2, 2, 24);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region Monobehaviour Messages
        // These methods are automatically called by Unity, you should remove any you aren't using.
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            
            Plugin.Log?.Debug($"{name}: Awake()");
            this._targetGO = new GameObject("Noctice");
            this._targetGO.transform.SetParent(Camera.main.transform, false);
            this._targetGO.transform.localPosition = PluginConfig.Instance.TargetOffset;
        }
        
        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {
            if (PluginConfig.Instance.Enable && this._nextShootTime < DateTime.Now) {
                this.Shoot();
                this._nextShootTime = DateTime.Now.AddSeconds(this._random.Next(this.minsec, this.maxsec));
            }
        }
        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            Destroy(this._ssCamera.gameObject);
            Destroy(this._targetGO);
        }
        #endregion
    }
}
