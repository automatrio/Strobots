using Godot;
using System.Collections.Generic;
using System;

public class Idle : State
{
    private Actor owner;

    public override void _Ready()
    {
        base._Ready();
        owner = (Actor)Owner;
    }

    public override void EnterState(Dictionary<string, object> msg)
    {
        (GetParent() as State).EnterState(msg);
    }

    public override void ExitState()
    {
        (GetParent() as State).ExitState();
    }

    public override void LocalInput(InputEvent @event)
    {
        (GetParent() as State).LocalInput(@event);
    }

    public override void PhysicsProcess(float delta)
    {
        var moveState = GetParent<Move>();
        
        if(owner.IsOnFloor())
        {
            if (moveState.GetDirection() != Vector3.Zero)
            {
                stateMachine.TransitionToState("Move/Run");
            }
        }
        else
        {
            stateMachine.TransitionToState("Move/Air");
        }
    }
}
