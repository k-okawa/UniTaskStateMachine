# UniTaskStateMachine

[[English](https://github.com/k-okawa/UniTaskStateMachine/blob/master/README.md)]

[![openupm](https://img.shields.io/npm/v/com.bg.unitaskstatemachine?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.bg.unitaskstatemachine/)

以下のように戻り値がUniTaskになっているので、非同期に対応したステートマシーンを利用することができます。
```c#
public interface IState
{
    void Init(BaseNode baseNode);
    UniTask OnEnter(CancellationToken ct = default);
    UniTask OnUpdate(CancellationToken ct = default);
    UniTask OnExit(CancellationToken ct = default);
}
```

そのため以下の特徴があります
- OnEnterが終了するまでOnUpdateのループが始まらない
- OnExitが終了するまで次のステートに移行しない
- OnUpdateは１フレームごとに呼ばれるがawaitしている間は呼ばれない

またステートの移行方法は以下を対応しています
- Transition(矢印)に判定を持たせ、条件が一致したときに移行
- TransitionIDを指定して次のステートに移行
  - 現在のステートが指定されたTransitionIDを持っていない場合は無視される
  
Editorツールも提供しているので、簡単にステートを組むことが可能です。

(スクリプト上で組むことも可能です)

![image](https://user-images.githubusercontent.com/49301086/183348165-6042c870-ac33-479a-b5fa-af210f345352.png)


## インストール
### 依存関係
パッケージ内でUniTaskを使用しているので、UniTaskがプロジェクト内に追加されている必要があります。

[UniTask](https://github.com/Cysharp/UniTask)

PackageManager,unitypackageのどちらで追加しても動作します。

UniRxから分割される前のUniTask(UniRx.Async)を使用している場合は、

ProjectSettings/Player/OtherSettings/ScriptingDefineSymbolで以下を定義することで使用可能になります。

```
BG_USE_UNIRX_ASYNC
```

### PackageManager

### GitHub経由でインストール

Window/Package Managerを開き、add package from git URL...で以下を入力して追加してください。

```
https://github.com/k-okawa/UniTaskStateMachine.git?path=Assets/Bg/UniTaskStateMachine
```

### OpenUPM経由でインストール

```
openupm add com.bg.unitaskstatemachine
```

### UnityPackage

[リリースページ](https://github.com/k-okawa/UniTaskStateMachine/releases)からダウンロード可能です。

## 使い方
### 1.StateMachineBehaviour追加
StateMachineBehaviourをAddComponentします。

![image](https://user-images.githubusercontent.com/49301086/143770544-d014aac1-e8a1-4c54-b1bf-d2945216f480.png)

### 2.GraphEditorを開く
1で追加したStateMachineBehaviourのGraphEditorOpen、

またはWindow/BG UniTaskStateMachine/StateMachineGraphでグラフエディタを開くことができます。

![image](https://user-images.githubusercontent.com/49301086/183349573-556b2bfb-968c-40ef-91b9-acde45bf5f65.png)

### 3.State追加方法
#### 3-1.StateBehaviourを追加
StateMachineBehaviourがアタッチされているGameObjectにBaseStateComponentを継承したComponentを追加します。

※BaseStateComponentを継承したクラスをさらに継承はしないでください。

※BaseStateComponentを直接AddComponentしないでください。

※同じStateComponentを同じゲームオブジェクトにAddComponentしないでください。

**例**
```c#
namespace Bg.UniTaskStateMachine.Tests.BasicSceneTest
{
    public class StartState : BaseStateComponent
    {
        public override async UniTask OnEnter(CancellationToken ct = default)
        {

        }

        public override async UniTask OnUpdate(CancellationToken ct = default)
        {

        }

        public override async UniTask OnExit(CancellationToken ct = default)
        {

        }
    }
}
```

![image](https://user-images.githubusercontent.com/49301086/143770911-aa150949-1ece-4ea1-833d-1de973e09c0e.png)

#### 3-2.追加したStateBehaviourを指定

Graphエディタ上で右クリックし、CreateStateでState追加

![image](https://user-images.githubusercontent.com/49301086/183352350-6de6bec5-b304-4a1a-b668-049c6841a9eb.png)

追加されているクラス名が表示され選択可能になります。

![image](https://user-images.githubusercontent.com/49301086/143771064-9915284a-fd38-4b00-b0f7-2a22c120c972.png)

Noneを選択した場合何もしないステートになります。

### 4.Transition追加方法
追加されているStateの上で右クリックし、MakeTransitionを選択することでTransitionを追加することができます。

![image](https://user-images.githubusercontent.com/49301086/143771406-b0e40166-fd07-4091-8e98-a5cac1ba83f2.png)

StateMachineBehaviourがアタッチされているGameObjectのComponentの中のpublicで戻り値がbool、引数なしの関数をステートの遷移条件として使用することができます。

![image](https://user-images.githubusercontent.com/49301086/143771235-8a12410e-21af-47d5-a7bb-bf1e9c03d1ae.png)

IsNegativeにチェックを入れることで条件を反対にすることができます。

またMethodNameの指定をNoneにすると、常に条件を満たさないTransitionになります。

後述するTriggerNextTransitionを呼び出すことでスクリプトから強制的に遷移を実行することも可能です。


![image](https://user-images.githubusercontent.com/49301086/143771269-6ffcd819-480a-4783-8f9f-ae28171c3036.png)

### 5.EntryState指定
最初に実行するStateを必ず指定する必要があります。

Stateを右クリックし、Set as Entryを選択することで設定することができます。

![image](https://user-images.githubusercontent.com/49301086/143771406-b0e40166-fd07-4091-8e98-a5cac1ba83f2.png)

### TriggerNextTransition

トランジションに条件を指定する以外にも、IDを指定してステートを遷移させることが可能です。

トランジションIDはC#のフィールド名で使用可能なアッパーキャメルケースを推奨します。

すべてのトランジションIDを決めた後、GenerateTransitionIdConstボタンを使用して定数のテンプレートコードを生成できます。

![image](https://user-images.githubusercontent.com/49301086/183354384-8c33ea1d-53a3-4bae-95f1-2011c0ddd660.png)

最後に、"StateMachine.TriggerNextTransition(string transitionId)"を呼び出すだけで好きなタイミングでステート遷移が可能です

引数に文字列を直接渡すことができますが、生成されたクラスの読み取り専用のフィールドで渡すことを推奨します。

## API Reference
### StateMachine
StateMachineBehaviourのStateMachineプロパティからアクセスできます。

#### プロパティ

```c#
// エントリーステート
public BaseNode EntryNode;

// 現在のステートノード
public BaseNode CurrentNode { get; private set; }

// 現在のStateMachineの実行状態(STOP,START,PAUSE)
public State CurrentState { get; private set; } = State.STOP;

// OnUpdateを呼ぶタイミング
public PlayerLoopTiming LoopTiming = PlayerLoopTiming.Update;
```

#### メソッド

```c#
/// <summary>
/// ステートマシンを開始する
/// </summary>
public async void Start();

/// <summary>
/// ステートマシーンを完全に停止する
/// </summary>
public void Stop();

/// <summary>
/// 現在のステートを一時停止状態にする
/// </summary>
public void Pause();

/// <summary>
/// 現在のステートを再開する
/// </summary>
public void Resume();

/// <summary>
/// エントリーステートからステートマシンを再実行
/// </summary>
public async UniTask ReStart(CancellationToken ct = default);

/// <summary>
/// 強制的に次のステートに遷移させる
/// </summary>
/// <param name="transitionId">Graphエディターで指定したtransitionId</param>
public void TriggerNextTransition(string transitionId);

/// <summary>
/// 現在実行中のステートと等しいかどうか調べる
/// </summary>
/// <param name="type">ステートタイプ</param>
/// <returns>現在のステートが引数のtypeと一致した場合trueを返す</returns>
public bool IsMatchCurrentStateType(Type type);

/// <summary>
/// ほとんどIsMatchCurrentStateTypeと同じ
/// 違いは引数が可変長引数になっていること
/// </summary>
/// <param name="types">ステートタイプ</param>
/// <returns>現在のステートが引数のいずれかのtypeと一致した場合trueを返す</returns>
public bool IsMatchAnyCurrentStateType(params Type[] types);
```

### BaseStateComponent
#### プロパティ

```c#
// 現在のステートのNode(StateとTransitionが一緒になっているもの)
protected BaseNode baseNode;
```

#### メソッド

```c#
public virtual void Init(BaseNode baseNode);
public virtual async UniTask OnEnter(CancellationToken ct = default);
public virtual async UniTask OnUpdate(CancellationToken ct = default);
public virtual async UniTask OnExit(CancellationToken ct = default);
```

### BaseNode

#### プロパティ

```c#
public readonly string Id;
public readonly StateMachine StateMachine;
public bool IsUpdate { get; private set; } = true;
```

#### メソッド

```c#
/// <summary>
/// いずれかの遷移条件がマッチしているか
/// </summary>
/// <returns>ひとつでも遷移条件にマッチしているものがあればtrueを返す</returns>
public bool IsMatchAnyCondition();

/// <summary>
/// TriggerNextTransitionによって強制的に遷移する状態になっているか
/// </summary>
/// <returns>ひとつでも強制的に遷移する状態のものがあればtrueを返す</returns>
public bool IsExistForceTransition();

/// <summary>
/// Graphエディター上の矢印(Transition)の基底クラスを取得する
/// </summary>
/// <param name="id">Graphエディター上のTransitionId</param>
public BaseCondition GetCondition(string id);

/// <summary>
/// Nodeが持っているTransitionのIdをすべて取得する
/// </summary>
public IEnumerable<string> GetTransitionIds();
```

### BaseCondition
#### プロパティ

```c#
public BaseNode NextNode { get; }
public Func<bool> ConditionCheckCallback { get; }
public string TransitionId { get; }
public bool IsNegative => isNegative;
public bool IsForceTransition => isForceTransition;
```
