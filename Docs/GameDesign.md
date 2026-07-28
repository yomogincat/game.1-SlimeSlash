# Game Design

## Overview

### Genre
2D Action Platformer

### Inspiration
- Hollow Knight

### Goal
プレイヤーがスライムを操作し、アクションを駆使してエリアを探索する。
多様なスキル、構成、育成要素
敵の体を収集して、自分に割り当てられるようにする
# Player

## Movement

### Walk
- 左右移動
- 加速度・減速度あり

### Jump
- ジャンプ
- コヨーテタイム対応
- ジャンプバッファ対応
- ジャンプカット対応

### Wall

- 壁張り付き
- 壁滑り
- 壁ジャンプ

### Dash

- 空中・地上で使用可能
- エネルギーを消費

### Attack

- 近距離斬撃
- ノックバックあり

---

# Energy

- 時間経過で回復
- ダッシュで消費

---

# Enemy

## Slime

### Behavior

- 左右巡回
- 壁で反転
- 崖で反転

### Status

- HPあり
- ノックバックあり
- 無敵時間あり

---

# Controls

| Action | Key |
|--------|-----|
| Move | A / D |
| Jump | Space |
| Dash | Q |
| Attack | J |

---

# Visual Style

- ドット絵
- スライムが主人公
- Hollow Knightのようなシンプルで見やすい表現

---

# Audio

- 斬撃SE
- 被弾SE
- ジャンプSE（予定）

---

# Future Ideas

- 新しい敵
- ボス
- 新しい攻撃
- エネルギーを使う特殊能力
- 敵のドロップ