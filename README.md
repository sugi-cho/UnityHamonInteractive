# UnityHamonInteractive

波紋シミュレーションの簡易マニュアル。

## 主要パラメータ（`RippleSimulation`）
- `resolution` : シミュレーション解像度。
- `waveSpeed` : 波速。高いほど広がりが速く強い。
- `timeScale` : 時間スケール。全体の進行を加速/減速。
- `damping` : 運動エネルギーの線形減衰（ダンパー）。
- `amplitudeDecay` : 振幅の時間減衰。残響を抑える。
- `depthScale` : 深度テクスチャによる波速変化の強さ。
- `flowScale` : フローテクスチャによる移流の強さ。
- `boundaryBounce` : 境界での反射係数。
- `forceToVelocity` : ブラシ/外力を速度に変換する係数。
- `horizontalEdge` / `verticalEdge` : テクスチャ端での処理モード（Bounce=反射, Absorb=吸収, Wrap=反対側へループ）。

### 時間ステップ
- `useFixedTimeStep` : 固定ステップを使うか（推奨オン）。
- `fixedTimeStep` : 1 ステップ時間（デフォルト 1/60s）。
- `maxSubSteps` : 1 フレームでの最大サブステップ数。
- `maxAccumulatedTime` : フレーム落ち時に積める上限時間。

## 入力テクスチャ
- `boundaryTexture` : 白=水面、黒=地形。反射/固体セル判定に使用。
- `depthTexture` : 0–1 の水深。深いほど波速が上がる。
- `flowTexture` : RG で XY 流速。水面の移流に使用。
- `externalForce` : 外部から与える力テクスチャ。`useExternalForce` が有効なとき加算。

## 内部RTと出力
- `State` : R=現在高さ, G=前フレーム高さ。
- `Result` : RGB=法線(0–1), A=高さ。`outputTexture` を設定すれば自動 Blit。
- `Force` : ブラシ／外力の蓄積用。

## ブラシ入力
`AddForceBrush(uv, radius, strength, falloff)` で力を与えられます。`forceToVelocity` と合わせて強度を調整してください。

## 注意
- `ProjectSettings/ProjectVersion.txt` で Unity 6000.2.8f1 を確認すること。
- 大きな生成物（Library, Temp, Logs など）はコミットしないでください。
