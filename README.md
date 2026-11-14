# AutoScreenShot

[![Build and Release](https://github.com/denpadokei/AutoScreenShot/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/AutoScreenShot/actions/workflows/build-and-release.yml)

ゲーム中に自動でスクリーンショットを撮影する MOD です。撮影はランダムな間隔・ランダムなカメラ位置で行われ、保存した画像をメインメニューに飾ることもできます。

## 機能

- プレイ中、設定した間隔の範囲でランダムに自動撮影
- 撮影のたびにカメラ位置と FOV をランダムに変更
- UI を除外する / HMD にのみ表示されるオブジェクトを含める を切り替え可能
- JPEG と PNG を選択でき、解像度・アンチエイリアスも指定可能
- メインメニューに撮影画像を浮かべて表示 (重なり方を切り替え可能)
- 設定画面から画像キャッシュを削除

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.39.1 |
| 依存 MOD | BSIPA 4.3.5 / SiraUtil 3.1.14 |

## インストール

1. Beat Saber を 1.39.1 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/AutoScreenShot/releases) から `AutoScreenShot-3.0.1-bs1.39.1.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

設定は Mod Settings の「AutoScreenShot」から開きます。

撮影した画像は `UserData/ScreenShoots/<yyyy_MM_dd>/BeatSaber_<yyyy_MM_dd_HH_mm_ss>.jpg` (PNG 設定時は `.png`) に保存されます。日付フォルダは撮影日ごとに作られます。

メインメニューへの表示を有効にすると、`UserData/ScreenShoots` 以下の JPEG / PNG がランダムに選ばれてメニュー空間に表示されます。枚数の上限は `Picture count` で変更でき、`Clear screen shot chache` ボタンで表示中の画像を消去できます。

## 設定

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| Enable | bool | true | 自動撮影を有効にするか |
| NoUI | bool | false | UI を撮影に含めない |
| IncludeHMDOnlyObject | bool | true | HMD にのみ表示されるオブジェクトを撮影に含める |
| MinSec | int | 6 | 撮影間隔の最小秒数 |
| MaxSec | int | 120 | 撮影間隔の最大秒数 |
| PositionScale | int | 10 | カメラ位置をランダムにずらす範囲 |
| MinFoV | float | 15 | カメラの FOV の最小値 |
| MaxFoV | float | 110 | カメラの FOV の最大値 |
| TargetOffset | Vector3 | (0, 0, 0) | 注視点の HMD からのオフセット |
| PictuerRenderSize | Vector2 | (1920, 1080) | 撮影する画像の解像度 |
| AntiAliasing | int | 1 | アンチエイリアス。1 / 2 / 4 / 8 のいずれか |
| ReadSplit | int | 4 | GPU からの読み戻しを何回に分割するか |
| Extention | ImageExtention | JPEG | 保存形式 (JPEG / PNG) |
| PictureCount | int | 100 | メニューに表示する画像の最大数。-1 で無制限 |
| ShowPictureInMenu | bool | true | メインメニューに撮影画像を表示するか |
| MenuPictureOverlap | bool | true | メニュー表示時に画像を重ねて配置するか |
| MenuPictuersMinRadius | float | 6.5 | メニューに表示する画像の配置半径の最小値 (設定ファイルには保存されない) |
| MenuPictuersMaxRadius | float | 8 | メニューに表示する画像の配置半径の最大値 (設定ファイルには保存されない) |

設定は `UserData/AutoScreenShot.json` に保存されます。撮影間隔は曲の長さに合わせて自動的にクランプされます。

## ビルド

```powershell
git clone https://github.com/denpadokei/AutoScreenShot.git
cd AutoScreenShot
dotnet build .\AutoScreenShot\AutoScreenShot.csproj -c Release
```

`BeatSaberDir` に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT
