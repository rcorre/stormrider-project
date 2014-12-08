using System.Linq;
using System.Collections.Generic;

public class StateMachine<T> {
    private Stack<State<T>> _stack;
    private T _owner;

    public StateMachine(T owner, State<T> firstState) {
        _owner = owner;
        _stack = new Stack<State<T>>();
        _stack.Push(firstState);
    }

    public State<T> CurrentState { get { return _stack.Peek(); } }

    public void Push(params State<T>[] states) {
        // calls exit() on current state if it was enter()ed
        CurrentState.Deactivate(_owner); 
        foreach (var state in states) {
            _stack.Push(state);
        }
    }

    public void Pop() {
        CurrentState.Exit(_owner);
        CurrentState.End(_owner);
        _stack.Pop();
    }

    public void Update() {
	// ensure state is started and entered
        CurrentState.Activate(_owner);
        CurrentState.Update(_owner);
    }
}

public abstract class State<T> {
    private bool _started, _entered;
    /// <summary>
    /// called once before first Enter()
    /// </summary>
    /// <param name="obj">object controlled by this state</param>
    public virtual void Start(T obj) { }

    /// <summary>
    /// called once after this state is popped (after Exit())
    /// </summary>
    /// <param name="obj">object controlled by this state</param>
    public virtual void End(T obj) { }

    /// <summary>
    /// called before Update each time this state becomes active
    /// </summary>
    /// <param name="obj">object controlled by this state</param>
    public virtual void Enter(T obj) { }

    /// <summary>
    /// called each time this state becomes inactive (before Enter() of next state)
    /// </summary>
    /// <param name="obj">object controlled by this state</param>
    public virtual void Exit(T obj) { }

    /// <summary>
    /// called once each update cycle while this state is active
    /// </summary>
    /// <param name="obj">object controlled by this state</param>
    public virtual void Update(T obj) { }

    /// <summary>
    /// called only by StateMachine before first update
    /// </summary>
    /// <param name="obj"></param>
    public void Activate(T obj) {
	if (!_started) {
	    Start(obj);
	    _started = true;
	}
	if (!_entered) {
	    Enter(obj);
	    _entered = true;
	}
    }

    /// <summary>
    /// called only by StateMachine when it is no longer the active state
    /// </summary>
    /// <param name="obj"></param>
    public void Deactivate(T obj) {
	if (_entered) {
	    Exit(obj);
	    _entered = false;
	}
    }
}
