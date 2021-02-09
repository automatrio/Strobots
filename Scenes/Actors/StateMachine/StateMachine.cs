using Godot;
using System.Collections.Generic;
using System;

public class StateMachine : Node
{
    // exports
    [Export] NodePath InitialState;
    
    // fields
    private State _currentState;

    // properties
    public State CurrentState
    {
        set
        {
            _currentState = value;
            if(Globals.CurrentPlayerEntity == Owner)
            {
                Globals.CurrentState = _currentState;
            }
        }
        get => _currentState;
    }
    public string StateName { get => _currentState.Name; }
    

    public StateMachine()
    {
        AddToGroup("StateMachine");
    }

    public override void _Ready()
    {
        GD.Print("StateMachine");
        // initial setup
        CurrentState = GetNode<State>(InitialState);
        CurrentState.EnterState(null);

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
        }
        else
        {
            TransitionToState(InitialState);
        }
    }
}
