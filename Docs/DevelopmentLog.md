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

## 2026-07-23
- HP用のオーブ画像（Full / Empty）を作成。
- CanvasにHP UIを追加し、Horizontal Layout Groupで自動配置。
- HPUI.csを作成し、PlayerのHPとオーブ表示を連携。
- HPオーブをPrefab化し、手動配置から動的生成へ変更。
- List<Image>とInstantiate()を用いて最大HPに応じたオーブ生成を実装。
- Transform.childCount・GetChild()を利用したUI管理を実装。
- 配列（Length）とList（Count）の違いを学習。
- 将来的な装備変更による最大HP・オーブデザイン変更に対応できる設計へ改善。

## 2026-7-24

- Enemyクラスを作成し、SlimeEnemy・RabbitEnemyが継承する構成に変更。
- 共通処理（HP、ノックバック、移動、地面・壁・プレイヤー判定、アニメーション、死亡処理）をEnemyへ移行。
- virtual / override / protected を使った継承構成を導入し、必要な処理だけ子クラスで上書きできるようにした。
- Directionプロパティを追加し、向きは外部から読み取り専用に変更。
- Slash.csの攻撃対象をSlimeEnemyからEnemyへ変更し、全ての敵へ攻撃できるようにした。
- RabbitEnemyのベースクラスを作成し、共通処理のみで動作する状態を確認。
- ウサギのドット絵を作成し、ゲーム用スプライトとして導入。
- 継承時のAwake / Start、base、protected、virtual、overrideの役割を整理・理解。
- 敵追加時はEnemyを継承し、固有AIのみ実装すればよい構成になった。

## 2026-7-28
- RabbitEnemyを追加し、Enemyを継承する形で新しい敵の実装を開始。
- ジャンプ前に停止する「ジャンプチャージ」処理を実装。
- isJumpCharging・isJumpingを用いた状態管理に変更し、ジャンプ処理を整理。
- ジャンプ中専用の移動速度（jumpMoveSpeed）を追加し、歩行速度と分離。
- OnLanded()を追加し、着地時の状態リセット処理を各敵で拡張できる設計に変更。
- Enemyの共通処理と各敵の固有処理を分離し、今後の敵追加がしやすい構成へ改善。

### 7/29
- Enemyクラスをリファクタリングし、GetMoveSpeed・CanTurn・OnLandedなど共通処理を整理。
- UpdateとFixedUpdateの役割を分離し、物理処理をFixedUpdateへ移動。
- Knockbackや移動速度の管理を見直し、各敵が必要な処理だけをoverrideできる構成に変更。
- HandleGroundCheckからOnLandedを呼び出す仕組みを追加し、着地時の処理を共通化。
- EnemyAttackを新規作成し、全敵で共通の攻撃判定を使用できるように変更。
- 攻撃ステータス（ダメージ・ノックバック等）をEnemyへ統合し、AttackBoxを共通化。
- Attack()・CanAttack()・OnAttack()・OnTouch()に役割を分割し、敵ごとの攻撃仕様を柔軟に変更できる設計へ改善。
- RabbitEnemyのジャンプAIを実装し、溜めジャンプ・空中移動・着地時クールタイムを追加。
- SlimeEnemy・RabbitEnemyともに共通クラスを利用する形へ整理し、新しい敵を追加しやすい構成に改善。

## 2026-07-30

- RabbitのRunアニメーションを作成（ホップする動きを意識して5フレーム構成に調整）。
- 重心移動と上下移動を見直し、「滑る」印象を改善。
- Jumpアニメーションを作成（上昇・下降を描き分け、ジャンプ状態が分かりやすいシルエットに変更）。
- 着地フレームはRunアニメーションの一部を再利用して自然なループを実現。
- Rabbitの目をBodyとは別オブジェクトで制御し、Animationで目線を動かす仕組みを実装。
- Unity Animationのキーフレーム編集・Recordモード・補間の仕組みを理解し、アニメーション編集方法を習得。

## 2026-07-31

- 壁キック時に稀に上昇せず落下するバグを確認。
- velocity *= jumpCutScale は原因ではなく、HandleGravity() を無効化するとバグが再現しないことを確認。
- jumpCutGravity 適用時のみ発生し、通常ジャンプでは再現しないことを確認。
- jumpCutBuffer・jumpCutEffectTimer・重力適用タイミングなど複数案を検証したが、根本原因は未特定。
- 当面は jumpCutGravity を廃止し、jumpCutScale による速度変更のみでJumpCutを実装。
- 壁ジャンプとJumpCutの状態遷移・重力処理は今後再調査予定。

## 2026-08-01

 新しい敵ではなく、既存敵の派生の設計を開始。
- Enemyの共通処理を見直し、ShouldTurn()からShouldTurnByPlayer()などの判断を分離し、継承しやすい構成へ改善。
- プレイヤー索敵処理を共通化し、前方・後方判定を追加できる設計を検討・実装。
- AlertSlimeを実装し、後方のプレイヤーを検知すると振り向く挙動を追加。
- AlertSlimeはダメージを受けた際にもチャージを開始し、段差下から一方的に攻撃されにくい挙動を実装。
- チャージ・方向転換などの責務を整理し、Handle・Should・On系メソッドの役割を明確化。
- スライム派生の見た目を区別するため、角やアクセサリーなどの装飾を追加する方針を決定。
- アニメーション管理は共通Controllerを基本とし、Animator Override Controllerの利用を検討。
- アセット命名規則を整理し、Sprite・Animation Clipは「Slime_Normal_Idle」のような「種類_状態」形式へ統一する方針を決定。

## 2026-08-04

- EnemyをGroundEnemyとFlyingEnemyに分離し、地上敵・飛行敵で役割を整理。
- FlyingEnemyを作成し、重力を無効化して水平移動の基盤を実装。
- BeeEnemyの巡回AIを実装し、ランダムなパトロール地点へ飛行する仕組みを追加。
- 速度を徐々に変化させる慣性付き移動を導入し、自然な飛行挙動を実現。
- パトロール地点を半径内で早めに更新することで、滑らかに折り返す飛行ルートへ改善。
- 飛行敵のAI構成を見直し、HandleAIを用いて移動・行動を管理する設計へ整理。
- プレイヤー検知にはDistance判定を採用し、追跡AIの基盤を設計。
- 追跡AIは「巡回→追跡→見失い→その場を中心に巡回」の流れで実装する方針を決定。

## 2026-08-05
- EnemyクラスをGroundEnemy・FlyingEnemyへ分割し、飛行敵用の基盤を作成した。
- FlyingEnemyでは重力無効化・壁反転・ノックバックなど共通の飛行処理を実装した。
- BeeEnemyを追加し、慣性付きの飛行移動（加速・減速）を実装した。
- パトロールAIを実装し、ランダムな目標地点へ巡回するようにした。
- プレイヤーの発見・追跡・見失いの基本AIを実装し、巡回と追跡を切り替えられるようにした。
- 追跡時のみ速度・加速度を変更できるようにし、より勢いのある飛行を実現した。
- AI・移動処理を整理し、HandleAIやGetMoveSpeedなど役割を分離してリファクタリングした。
- Hollow KnightのHiveのハチを参考に、目標更新や慣性を調整しながら挙動を改善した。
- Y方向の加速度比率を調整し、より自然な空中からの襲撃感に近づけた。

## 2026-08-08

- SpawnPointの実装を開始し、EnemyPrefab・生成位置・初期方向・リスポーン時間などを設定できる構成にした。
- SpawnPointからEnemyをInstantiateし、Initialize(direction)で生成時の向きを設定する仕組みを実装。
- EnemyのOnDieイベントを利用し、敵の死亡をSpawnPoint側で検知できるようにした。
- 敵死亡時にSpawnPointが保持しているEnemy参照をnullにし、リスポーン可能な状態へ戻す処理を追加。
- プレイヤーとSpawnPointの距離を判定するIsPlayerInRange()を実装し、一定範囲内でのみ敵をSpawnする仕組みを作成。
- プレイヤーが範囲外にいる間だけリスポーンタイマーを進め、範囲内に戻った際に敵を生成する方式を採用。
- ゲーム開始時にもIsPlayerInRange()を確認し、最初からプレイヤー付近にあるSpawnPointのみ敵を生成するよう変更。
- SpawnPointの位置・敵名・リスポーンタイマーをOnDrawGizmosSelected()でSceneビューに表示できるようにした。
- 特殊なSpawnPoint（複数体生成、ダメージを受けたハチの巣からBeeを生成など）にも対応できるよう、Spawn()を外部から呼び出せる設計を検討。
- 今後は画面内への突然の出現を避けるため、カメラ範囲を考慮したスポーン条件や、復活時の敵配置を調整する予定。

## 2026-08-12

- テントウムシ（LadybugEnemy）の実装を開始。
- イカリバエを意識し、基本は動かず、プレイヤーを発見すると追跡するシンプルなAIに決定。
- プレイヤー発見時はtakeOffSpeed・takeOffTimeで短時間上昇してから追跡する構成を実装。
- 追跡中は重力を無効化し、追跡終了後はfallSpeedでゆっくり降下する方針を決定。
- GroundEnemyのOnLanded()を利用して着地処理を行えることを確認。
- 飛びすぎを防ぐため、飛行中のY方向速度を制限するmaxFlySpeedYを検討。
- LadybugのEyeを2つにし、既存のEye Animationを左右で共有する方針を決定。
- EnemyのeyeAnimをAnimator[]に変更し、複数のEye AnimatorへSetAnim()から同じパラメータを送れるように変更。
- 被ダメージSEのフィールド名をhitからhitSEへ変更し、SerializeFieldの名前変更で素材参照がNoneになることを確認。
- Enemy共通の被ダメージSEとしてAudioSourceとhitSEをEnemy側で管理する構成を継続。

## 2026-08-13

- Hollow Knightのカメラ挙動を参考に、プレイヤーが向いている方向へカメラを少しずらすLook Aheadを実装。
- PlayerのDirectionを外部から読み取り専用で取得し、CameraControlからプレイヤーの向きを利用できる構成を確認。
- Look Aheadの目標値をDirection × lookAheadDistanceで決定し、currentLookAheadをLerpで滑らかに変化させる方式を採用。
- 通常のカメラ追従速度とLook Aheadの変化速度を分離し、followSpeedとlookAheadSpeedを個別に調整できるようにした。
- Look Aheadの調整を行い、lookAheadSpeedは0.2～0.25程度から良好な挙動を確認。
- カメラの水平方向の位置関係を確認し、Hollow Knightではプレイヤーの背中側と前方側の見える範囲がおよそ5:7になることを確認。
- 水平・垂直方向にデッドゾーンを追加し、プレイヤーが一定範囲内にいる間はカメラが追従しない構成を実装。
- デッドゾーンはtargetPositionを中心としてMathf.Clampでカメラ位置を制限する方式を採用し、deadZoneX・deadZoneYを個別に調整できるようにした。
- 水平方向のデッドゾーンは幅3程度（片側1.5程度）を基準に調整し、実際のプレイで良好な挙動を確認。
- CameraControlの処理を「Look Ahead → Target Position → Follow → Dead Zone → カメラ範囲のClamp」の流れに整理し、現時点のカメラ仕様を確定。