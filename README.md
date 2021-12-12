# UniTaskStateMachine

[[日本語]()]

[![openupm](https://img.shields.io/npm/v/com.littlebigfun.addressable-importer?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.littlebigfun.addressable-importer/)

Since the return value is UniTask as shown below, you can use a state machine that supports asynchronous.
```c#
public interface IState
{
    void Init(BaseNode baseNode);
    UniTask OnEnter(CancellationToken ct = default);
    UniTask OnUpdate(CancellationToken ct = default);
    UniTask OnExit(CancellationToken ct = default);
}
```

It has the following features.
- The OnUpdate loop does not start until OnEnter finishes
- Does not transition to the next state until OnExit finishes
- OnUpdate is called every frame but not during await

In addition, the state transition method corresponds to the following.
- Transition when conditions are match
- Transition to the next state by specifying the TransitionID
  - Ignored if the current state does not have the specified TransitionID
  
This package also provide an Editor tool, so you can easily create states.

![image](https://user-images.githubusercontent.com/49301086/137613577-d510a77c-0231-4e76-bf0f-a6f16a2ae506.png)


## Installation
### Dependency
Using UniTask in the package, UniTask must be added in the project.

[UniTask](https://github.com/Cysharp/UniTask)

If you are using UniTask (UniRx.Async) before it was split from UniRx

It can be used by defining the following in ProjectSettings / Player / OtherSettings / ScriptingDefineSymbol.

```
BG_USE_UNIRX_ASYNC
```

### PackageManager

#### Install via git url

Open Window/Package Manager, and add package from git URL...

```
https://github.com/k-okawa/UniTaskStateMachine.git?path=Assets/Bg/UniTaskStateMachine
```

#### Install via OpenUPM

```
openupm add com.bg.unitaskstatemachine
```

## How to use
### 1.Add StateMachineBehaviour Component

![image](https://user-images.githubusercontent.com/49301086/143770544-d014aac1-e8a1-4c54-b1bf-d2945216f480.png)

### 2.Open GraphEditor

GraphEditorOpen of StateMachineBehaviour added in 1

Or you can open the graph editor with Window / BG UniTaskStateMachine / StateMachineGraph.

![image](https://user-images.githubusercontent.com/49301086/143770686-8efd36c8-35fc-40a5-a1ec-4862a2dda9e3.png)

### 3.How to add state
#### 3-1.Add StateBehaviour

Add a Component that inherits BaseStateComponent to the GameObject to which StateMachineBehaviour is attached.


* Do not inherit the class that inherits BaseStateComponent.

* Do not addBaseStateComponent directly.

**Example**
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

#### 3-2.Set the added StateBehaviour

Right-click on the Graph editor and select Create State to add State.

![image](https://user-images.githubusercontent.com/49301086/143770989-faa1a688-2ecd-4407-87d7-3e9b2a4c570a.png)

The added class name will be displayed and can be selected.

![image](https://user-images.githubusercontent.com/49301086/143771064-9915284a-fd38-4b00-b0f7-2a22c120c972.png)

If None is selected, the state will be nothing.

### 4.How to add Transition

You can add a Transition by right-clicking on the added State and selecting Make Transition.

![image](https://user-images.githubusercontent.com/49301086/143771406-b0e40166-fd07-4091-8e98-a5cac1ba83f2.png)


You can use a function that is public in the Component of the GameObject to which StateMachineBehaviour is attached, has a return value of bool, and has no arguments, as a state transition condition.

![image](https://user-images.githubusercontent.com/49301086/143771235-8a12410e-21af-47d5-a7bb-bf1e9c03d1ae.png)

You can reverse the condition by checking Is Negative.

If you set MethodName to None, the transition will not always meet the conditions.


It is also possible to forcibly execute the transition from the script by calling TriggerNextTransition described later.


![image](https://user-images.githubusercontent.com/49301086/143771269-6ffcd819-480a-4783-8f9f-ae28171c3036.png)

### 5.EntryState specification
You must always specify the State to execute first.

It can be set by right-clicking on the State and selecting Set as Entry.

![image](https://user-images.githubusercontent.com/49301086/143771406-b0e40166-fd07-4091-8e98-a5cac1ba83f2.png)

## API Reference
### StateMachine
It can be accessed from the StateMachine property of the StateMachineBehaviour.

#### Properties

```c#
// Entry state
public BaseNode EntryNode;

// Current state node
public BaseNode CurrentNode { get; private set; }

// current StateMachine state(STOP,START,PAUSE)
public State CurrentState { get; private set; } = State.STOP;

// Timing of OnUpdate
public PlayerLoopTiming LoopTiming = PlayerLoopTiming.Update;
```

#### Methods

```c#
/// <summary>
/// Start StateMachine
/// </summary>
public async void Start();

/// <summary>
/// stop state machine completely
/// </summary>
public void Stop();

/// pause current state
/// </summary>
public void Pause();

/// <summary>
/// resume current state
/// </summary>
public void Resume();

/// <summary>
/// restart state machine from entry state
/// </summary>
public async UniTask ReStart(CancellationToken ct = default) 

/// <summary>
/// force transition to next state
/// </summary>
/// <param name="transitionId">transition id named on graph editor</param>
public void TriggerNextTransition(string transitionId) 

/// <summary>
/// whether current state is equivalent
/// </summary>
/// <param name="type">state type</param>
/// <returns>return true if current state is type argument</returns>
public bool IsMatchCurrentStateType(Type type) 

/// <summary>
/// almost same with IsMatchCurrentStateType
/// difference is variable length arguments
/// </summary>
/// <param name="types">state types</param>
/// <returns>return true if current state match with any type arguments</returns>
public bool IsMatchAnyCurrentStateType(params Type[] types) 
```

### BaseStateComponent
#### Properties

```c#
// Crrent node that has state and transition
protected BaseNode baseNode;
```

#### Methods

```c#
public virtual void Init(BaseNode baseNode)
public virtual async UniTask OnEnter(CancellationToken ct = default)
public virtual async UniTask OnUpdate(CancellationToken ct = default)
public virtual async UniTask OnExit(CancellationToken ct = default)
```

### BaseNode

#### Properties

```c#
public readonly string Id;
public readonly StateMachine StateMachine;
public bool IsUpdate { get; private set; } = true;
```

#### Methods

```c#
/// <summary>
/// is match any condition
/// </summary>
/// <returns>return true if node has any true conditions</returns>
public bool IsMatchAnyCondition()

/// <summary>
/// is exist force transition
/// </summary>
/// <returns>return true if node has any force transition</returns>
public bool IsExistForceTransition()

/// <summary>
/// get condition
/// </summary>
/// <param name="id">condition id (transition id named on graph editor)</param>
/// <returns>base condition</returns>
public BaseCondition GetCondition(string id)

/// <summary>
/// get transition ids that node has
/// </summary>
public IEnumerable<string> GetTransitionIds()
```

### BaseCondition
#### Properties

```c#
public BaseNode NextNode { get; }
public Func<bool> ConditionCheckCallback { get; }
public string TransitionId { get; }
public bool IsNegative => isNegative;
public bool IsForceTransition => isForceTransition;
```
