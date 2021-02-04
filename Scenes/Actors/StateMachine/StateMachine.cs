using Godot;
using System.Collections.Generic;
using System;

public class StateMachine : Node
{
    [Export] NodePath InitialState;
    [Export] NodePath DebugControlPath;
    private DebugControl _debugControl;
    public State CurrentState
    {
        set
        {
            _currentState = value;
            _stateName = CurrentState.Name;
        }
        get => _currentState;
    }
    private State _currentState;
    private string _stateName; 
    public StateMachine()
    {
        AddToGroup("StateMachine");
    }
    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        CurrentState = GetNode<State>(InitialState);
        await ToSignal(Owner, "ready");
        CurrentState.EnterState(null);

        _debugControl = GetNode<DebugControl>(DebugControlPath);
    }
    public override void _Input(InputEvent @event)
    {
        CurrentState.LocalInput(@event);
    }
    public override void _PhysicsProcess(float delta)
    {
        CurrentState.PhysicsProcess(delta);
    }

    public void TransitionToState(
        NodePath targetStatePath,
        Dictionary<string, object> message = null)
    {
        if(message == null)
        {
            message = new Dictionary<string, object>();
        }

        if(HasNode(targetStatePath))
        {
            var targetState = GetNode<State>(targetStatePath);
            CurrentState.ExitState();
            CurrentState = targetState;
            CurrentState.EnterState(message);
            _debugControl.DisplayCurrentState(_stateName);
        }
        else
        {
            _debugControl.DisplayExceptionMessage("Couldn't transition to specified state.");
        }
    }
}
