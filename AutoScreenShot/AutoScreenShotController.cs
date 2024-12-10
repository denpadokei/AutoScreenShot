using AutoScreenShot.Configuration;
using AutoScreenShot.Extention;
using SiraUtil.Zenject;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace AutoScreenShot
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class AutoScreenShotController : MonoBehaviour, IAsyncInitializable
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
        public async Task InitializeAsync(CancellationToken token)
        {
            this._minFoV = PluginConfig.Instance.MinFoV;
            this._maxFoV = PluginConfig.Instance.MinFoV < this._minFoV ? this._minFoV : PluginConfig.Instance.MaxFoV;

            this._posScale = PluginConfig.Instance.PositionScale;
            while (Camera.main == null || Camera.main.gameObject == null) {
                await Task.Yield();
            }
            this._ssCamera = Instantiate(Camera.main.gameObject).GetComponent<Camera>();
            this._ssCamera.gameObject.name = "Prompt";
            this._ssCamera.gameObject.tag = "Untagged";
            this._ssCamera.name = "Prompt Camera";
            this._ssCamera.stereoTargetEye = StereoTargetEyeMask.None;
            this._ssCamera.gameObject.transform.position = new Vector3(0f, 1.7f, -3.2f);
            this._ssCamera.cullingMask = -1;
#if DEBUG
            foreach (var cam in Resources.FindObjectsOfTypeAll<Camera>()) {
                Plugin.Log.Debug($"{cam.name} : {cam}");
            }
#endif
            this._targetGO.transform.SetParent(Camera.main.transform, false);
            this._targetGO.transform.localPosition = PluginConfig.Instance.TargetOffset;
            if (PluginConfig.Instance.NoUI) {
                this._ssCamera.cullingMask &= ~(1 << s_ui_Layer);
            }
            if (!PluginConfig.Instance.IncludeHMDOnlyObject) {
                this._ssCamera.cullingMask &= ~(1 << s_hmdOnly_Layer);
            }
            this._ssCamera.depthTextureMode = (DepthTextureMode.Depth | DepthTextureMode.DepthNormals | DepthTextureMode.MotionVectors);
            this._ssCamera.targetTexture = new RenderTexture(2, 2, 24);
            this._ssCamera.enabled = false;
            this._saveType = PluginConfig.Instance.Extention;
#if DEBUG
            foreach (var item in this._ssCamera.gameObject.GetComponentsInChildren<MonoBehaviour>()) {
                Plugin.Log.Debug($"{item}");
            }
#endif

            if (!SystemInfo.supportsAsyncGPUReadback) {
                this._isSupprot = false;
                Plugin.Log.Debug("Not supproted.");
                return;
            }
            this._isSupprot = true;
            if (!Directory.Exists(s_dataDir)) {
                Directory.CreateDirectory(s_dataDir);
            }
            this._width = (int)PluginConfig.Instance.PictuerRenderSize.x;
            this._height = (int)PluginConfig.Instance.PictuerRenderSize.y;
            this._antiAliasing = PluginConfig.Instance.AntiAliasing;
            this._readSplit = PluginConfig.Instance.ReadSplit < 1 ? 1 : PluginConfig.Instance.ReadSplit;
            this._splitHeight = (int)(PluginConfig.Instance.PictuerRenderSize.y / this._readSplit);
            switch (this._saveType) {
                case ImageExtention.JPEG:
                    this._saveByteData = new byte[this._width * this._height * 4];
                    break;
                case ImageExtention.PNG:
                    this._saveColorData = new Color32[this._width * this._height];
                    break;
                default:
                    break;
            }

#if DEBUG
            this._nextShootTime = DateTime.Now.AddSeconds(10);
#else
            this._nextShootTime = DateTime.Now.AddSeconds(this._random.Next(this._minsec, this._maxsec));
#endif
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void SetupShot()
        {
            if (!this._isSupprot) {
                return;
            }
            this.SetCameraPos(this.CreateCameraPos());
            var aa = s_aaNums.Contains(this._antiAliasing) ? this._antiAliasing : 1;
            this._colorTexture = RenderTexture.GetTemporary(this._width, this._height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, aa);
            this._oldtextuer = this._ssCamera.targetTexture;
            this._ssCamera.targetTexture = this._colorTexture;
            this._isSetup = true;
            this._isReadSave = true;
            this._saveCount = 0;
            this._saveDataSize = 0;
            this._ssCamera.Render();
        }
        private void ReadbackShot()
        {
            var offsetHeight = this._splitHeight * this._saveCount;
            this._saveCount++;
            var height = this._splitHeight;
            if (this._readSplit <= this._saveCount) {
                height = this._colorTexture.height - offsetHeight;
            }
            AsyncGPUReadback.Request(this._colorTexture, 0, 0, this._colorTexture.width, offsetHeight, height, 0, 1, async req =>
            {
                if (req.hasError || !this._isReadSave) {
                    return;
                }
                switch (this._saveType) {
                    case ImageExtention.JPEG:
                        using (var nativeData = req.GetData<byte>()) {
                            NativeArray<byte>.Copy(nativeData, 0, this._saveByteData, this._saveDataSize, nativeData.Length);
                            this._saveDataSize += nativeData.Length;
                        }
                        break;
                    case ImageExtention.PNG:
                        using (var colorBuffer = req.GetData<Color32>()) {
                            NativeArray<Color32>.Copy(colorBuffer, 0, this._saveColorData, this._saveDataSize, colorBuffer.Length);
                            this._saveDataSize += colorBuffer.Length;
                        }
                        break;
                    default:
                        break;
                }
                if (this._readSplit <= this._saveCount) {
                    await this.SaveShot();
                    RenderTexture.ReleaseTemporary(this._colorTexture);
                    this._isReadSave = false;
                }
                else {
                    this.ReadbackShot();
                }
            });
        }
        private async Task SaveShot()
        {
            switch (this._saveType) {
                case ImageExtention.JPEG:
                    await Task.Run(() =>
                    {
                        try {
                            var jpgBytes = ImageConversion.EncodeArrayToJPG(this._saveByteData, this._colorTexture.graphicsFormat, (uint)this._colorTexture.width, (uint)this._colorTexture.height);
                            if (!Directory.Exists(s_dataDir)) {
                                Directory.CreateDirectory(s_dataDir);
                            }
                            File.WriteAllBytes(Path.Combine(s_dataDir, $"BeatSaber_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.jpg"), jpgBytes);
                        }
                        catch (Exception e) {
                            Plugin.Log.Error(e);
                        }
                    });
                    break;
                case ImageExtention.PNG:
                    await Task.Run(() =>
                    {
                        try {
                            for (var i = 0; i < this._saveColorData.Length; i++) {
                                this._saveColorData[i].a = 255;
                            }
                            var pngBytes = ImageConversion.EncodeArrayToPNG(this._saveColorData, this._colorTexture.graphicsFormat, (uint)this._colorTexture.width, (uint)this._colorTexture.height);
                            if (!Directory.Exists(s_dataDir)) {
                                Directory.CreateDirectory(s_dataDir);
                            }
                            File.WriteAllBytes(Path.Combine(s_dataDir, $"BeatSaber_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png"), pngBytes);
                        }
                        catch (Exception e) {
                            Plugin.Log.Error(e);
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        private void SetCameraPos(Vector3 pos)
        {
            this._ssCamera.transform.position = pos;
            this._ssCamera.fieldOfView = this._random.NextFloat(this._minFoV, this._maxFoV);
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
        private readonly GameObject _targetGO = new GameObject("Noctice");
        private bool _isSupprot;
        private static readonly string s_dataDir = Path.Combine(Environment.CurrentDirectory, "UserData", "ScreenShoots", $"{DateTime.Now:yyyy_MM_dd}");
        private DateTime _nextShootTime;
        private int _width;
        private int _height;
        private int _antiAliasing = 1;
        private System.Random _random;
        private int _minsec;
        private int _maxsec;
        private float _minFoV;
        private float _maxFoV;
        private float _posScale;
        private ImageExtention _saveType;
        private static readonly int[] s_aaNums = { 1, 2, 4, 8 };
        private const int s_ui_Layer = 5;
        private const int s_hmdOnly_Layer = 6;
        private RenderTexture _colorTexture;
        private RenderTexture _oldtextuer;
        private bool _isSetup = false;
        private bool _isReadSave = false;
        private byte[] _saveByteData;
        private Color32[] _saveColorData;
        private int _saveDataSize = 0;
        private int _saveCount = 0;
        private int _splitHeight;
        private int _readSplit = 4;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        private void Constractor(IAudioTimeSource controller)
        {
            this._random = new System.Random(Environment.TickCount);
            this._minsec = controller.songEndTime < PluginConfig.Instance.MinSec ? (int)(controller.songEndTime / 2) : PluginConfig.Instance.MinSec;
            this._maxsec = controller.songEndTime < PluginConfig.Instance.MaxSec || PluginConfig.Instance.MinSec < this._minsec ? (int)controller.songEndTime : PluginConfig.Instance.MaxSec;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region Monobehaviour Messages
        // These methods are automatically called by Unity, you should remove any you aren't using.
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
#if DEBUG
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);
            foreach (var cam in Resources.FindObjectsOfTypeAll<Camera>()) {
                Plugin.Log.Debug($"{cam.name} : {cam}");
            }
        }
#endif
        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {
            if (this._isSetup) {
                this._isSetup = false;
                this._ssCamera.targetTexture = this._oldtextuer;
                this.ReadbackShot();
                return;
            }
            if (!this._isReadSave && PluginConfig.Instance.Enable && this._nextShootTime < DateTime.Now) {
                this.SetupShot();
                this._nextShootTime = DateTime.Now.AddSeconds(this._random.Next(this._minsec, this._maxsec));
            }
        }
        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (this._isReadSave) {
                RenderTexture.ReleaseTemporary(this._colorTexture);
            }
            this._isReadSave = false;
            Plugin.Log?.Debug($"{this.name}: OnDestroy()");
            Destroy(this._ssCamera.gameObject);
            Destroy(this._targetGO);
        }
        #endregion
    }
}
