using Godot;
using System.Collections.Generic;
using System;

public abstract class State : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public abstract void PhysicsProcess(float delta);
    public abstract void LocalInput(InputEvent @event);
    public abstract void EnterState(Dictionary<string, object> msg);
    public abstract void ExitState();
    public Node GetStateMachine(Node node)
    {
        if (node != null && !node.IsInGroup("StateMachine"))
        {
            return GetStateMachine(node.GetParent());
        }
        else
        {
            return node;
        }
    }

    public StateMachine stateMachine;

    public override void _Ready()
    {
        stateMachine = (StateMachine)GetStateMachine(this);
    }
}
