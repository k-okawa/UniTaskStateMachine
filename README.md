# UniTaskStateMachine
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

![image](https://user-images.githubusercontent.com/49301086/137613577-d510a77c-0231-4e76-bf0f-a6f16a2ae506.png)


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
Window/Package Managerを開き、add package from git URL...で以下を入力して追加してください。

```
https://github.com/k-okawa/UniTaskStateMachine.git?path=Assets/Bg/UniTaskStateMachine
```
