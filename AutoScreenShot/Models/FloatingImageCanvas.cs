using AutoScreenShot.Extention;
using HMUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AutoScreenShot.Models
{
    public class FloatingImageCanvas : MonoBehaviour
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        public ImageView Image { get; set; }
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
        public async void Init(string filePath)
        {
            this._imageFilePath = filePath;
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }
            if (!File.Exists(this._imageFilePath)) {
                return;
            }
            byte[] datas = null;
            await Task.Run(() =>
            {
                datas = File.ReadAllBytes(this._imageFilePath);
            }).ConfigureAwait(true);
            var textuer = this.CreateTextuer2D(datas);
            if (datas == null) {
                return;
            }
            this.GetImageSize(datas, out var width, out var height);
            (this._rootCanvas.transform as RectTransform).sizeDelta = new Vector2(width * 2, height * 2);
            this._rootCanvas.transform.localScale = Vector3.one * 0.001f;
            var localScale = this._rootCanvas.transform.localScale;
            this._rootCanvas.transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            if (this.Image != null) {
                Destroy(this.Image.gameObject);
            }
            this.Image = new GameObject("FloatingImage", typeof(ImageView)).GetComponent<ImageView>();
            this.Image.rectTransform.SetParent(this._rootCanvas.transform as RectTransform, false);
            this.Image.rectTransform.sizeDelta = new Vector2(width, height);
            this.Image.transform.localPosition = Vector3.zero;
            this.Image.material = _noGlow;
            this.Image.material.mainTexture = textuer;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void Move()
        {
            this._rootCanvas.transform.position = new Vector3
                (
                this._rootCanvas.transform.position.x,
                this._rootCanvas.transform.position.y + Mathf.Sin(Time.time * 0.5f) / 1000,
                this._rootCanvas.transform.position.z
                );
        }

        private Texture2D CreateTextuer2D(byte[] datas)
        {
            this.GetImageSize(datas, out var width, out var height);
            var result = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            result.LoadImage(datas);
            return result;
        }

        private void GetImageSize(byte[] datas, out int width, out int height)
        {
            width = 0;
            height = 0;

            for (int i = 0; i < datas.Length; i++) {
                if (datas[i] == 0xff) {
                    if (datas[i + 1] == 0xc0) {
                        height = datas[i + 5] * 256 + datas[i + 6];
                        width = datas[i + 7] * 256 + datas[i + 8];
                        break;
                    }
                }
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        private static readonly float _maxheight = 2f;
        private static readonly float _minheight = -2f;
        private static readonly float _moveSec = 4;
        private float _speed;
        private string _imageFilePath;
        private Canvas _rootCanvas;
        private Material _noGlow;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // Unity methods
        public void Update()
        {
            this.Move();
        }
        public void Awake()
        {
            if (_noGlow == null) {
                _noGlow = Instantiate(Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "UINoGlow"));
            }
            this._rootCanvas = this.gameObject.AddComponent<Canvas>();
            this.gameObject.AddComponent<CurvedCanvasSettings>();
            this._rootCanvas.renderMode = RenderMode.WorldSpace;
            
            this._speed = (_maxheight - _minheight) / _moveSec;
            Plugin.Log.Debug($"{this._speed}");
        }
        #endregion
        [Flags]
        public enum Sequence {
            Stop = 1,
            Move = 1 << 1,
            Up = 1 << 2,
            Down = 1 << 3,
        }
    }
}
