# Development Log

## 2026-07-16
- テスト

## 2026-07-17

- Slashエフェクトのドット絵・アニメーション（4フレーム）を作成し、Animatorを追加。
- Enemyレイヤーを作成し、敵同士が衝突しないように設定。
- SlimeEnemy.csの変数を役割ごとに整理し、不要な変数を削除。
- Player検知用Raycast（playerRay / playerLayer / isPlayerAhead）を追加。
- 移動速度を walkSpeed と chargeSpeed に分ける設計に変更。
- ダメージ・ノックバックなど攻撃用パラメータを整理。
- PlayerはHitDamage()のみ担当し、Enemy側が攻撃内容を決める設計を採用。
- 接触ダメージは当面共通仕様とし、将来必要なら敵ごとの差別化を検討。
- 次回：Charge状態・接触ダメージ・敵の攻撃処理を実装。

## 2026-07-18
- ColliderとCollisionの役割を整理し、接触判定と攻撃判定の使い分けを検討。
- 事故当たりと攻撃当たりを分離し、敵側でHitDamageを呼び出す設計を採用。
- ノックバック処理の責務を見直し、敵ごとにノックバック性能を持たせる方針を決定。
- wallJump関連のタイマー制御を確認し、入力受付を時間制で管理する案を検討。
- ジャンプバッファ・ダッシュバッファなど、入力バッファ設計について整理。

## 2026-07-20
- スライムのチャージ攻撃を実装し、プレイヤー発見・見失い時の状態遷移とChargeTimerを追加。
- OnCollisionEnter2Dを用いた接触ダメージを実装し、通常接触とチャージ攻撃で処理を分離。
- ノックバック処理を改善し、HitDamage()で初速を一度だけ与える方式へ変更して物理挙動を自然化。
- プレイヤー・敵ともに被弾時の責務を整理し、被弾側がダメージ・無敵時間・SEを管理する方針を決定。
- ヒットストップをCoroutineで実装し、効果音と合わせて約0.08〜0.10秒が最適であることを確認。
- スライムの目用Animatorを追加し、GraphicsとEyeのAnimatorを共通メソッドで制御するよう整理。
- チャージ中のみ目を変化させる演出を追加し、攻撃予兆を視覚的に分かりやすくした。
- Animationフォルダ構成を見直し、共通クリップとキャラクター固有Controllerを分離する方針を決定。
- 操作感向上のためキー配置を検討し、ダッシュ権方式（canDash/dashCount）の設計も検討開始。

## 2026-07-21

- プレイヤー・敵の当たり判定をTriggerベースへ移行開始。
- Playerの子にHitBox(Trigger)を追加し、被弾判定を本体Colliderと分離する構成を採用。
- AttackBox・HitBox・BodyColliderの役割を整理し、接触ダメージと攻撃判定を分離する設計を決定。
- Rigidbody2DはPlayer・Enemy本体のみが持ち、HitBox・AttackBoxは親のRigidbody2Dを利用してTrigger判定を行う方針を決定。
- レイヤー構成を見直し、Player / PlayerHit / PlayerAttack / Enemy / EnemyHit / EnemyAttackへ整理する方針を決定。
- Layer Collision Matrixは必要な組み合わせのみ有効化する運用とし、不要な衝突判定を削減する方針を決定。
- Hierarchy構成を見直し、Graphics・Checks・Combat(Triggers)など役割ごとにEmptyで整理する方針を検討。
- AttackPointはGraphicsの反転に追従させるため、Graphics配下に配置する構成を維持。
- 引数が多い関数は改行して可読性を高めるコーディングルールを採用。
- ローカル変数は使用する関数内で宣言する方針を確認。
- Player無敵時間中の点滅演出を検討し、HandleInvincibleEffect()による専用処理とColorを用いた点滅方式を設計（未実装）。

## 2026/07/23
- HP用のオーブ画像（Full / Empty）を作成。
- CanvasにHP UIを追加し、Horizontal Layout Groupで自動配置。
- HPUI.csを作成し、PlayerのHPとオーブ表示を連携。
- HPオーブをPrefab化し、手動配置から動的生成へ変更。
- List<Image>とInstantiate()を用いて最大HPに応じたオーブ生成を実装。
- Transform.childCount・GetChild()を利用したUI管理を実装。
- 配列（Length）とList（Count）の違いを学習。
- 将来的な装備変更による最大HP・オーブデザイン変更に対応できる設計へ改善。

## 7/24 敵クラス共通化・新敵追加準備

- Enemyクラスを作成し、SlimeEnemy・RabbitEnemyが継承する構成に変更。
- 共通処理（HP、ノックバック、移動、地面・壁・プレイヤー判定、アニメーション、死亡処理）をEnemyへ移行。
- virtual / override / protected を使った継承構成を導入し、必要な処理だけ子クラスで上書きできるようにした。
- Directionプロパティを追加し、向きは外部から読み取り専用に変更。
- Slash.csの攻撃対象をSlimeEnemyからEnemyへ変更し、全ての敵へ攻撃できるようにした。
- RabbitEnemyのベースクラスを作成し、共通処理のみで動作する状態を確認。
- ウサギのドット絵を作成し、ゲーム用スプライトとして導入。
- 継承時のAwake / Start、base、protected、virtual、overrideの役割を整理・理解。
- 敵追加時はEnemyを継承し、固有AIのみ実装すればよい構成になった。