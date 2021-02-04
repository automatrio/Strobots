using Godot;
using System.Collections.Generic;
using System;

public class Run : State
{
    private Actor owner;
    private Move move;
    public override void _Ready()
    {
        base._Ready();
        owner = Owner as Actor;
        move = (GetParent() as Move);
    }
    public override void EnterState(Dictionary<string, object> msg)
    {
        move.EnterState(msg);
    }

    public override void ExitState()
    {
        move.ExitState();
    }

    public override void LocalInput(InputEvent @event)
    {
        move.LocalInput(@event);
    }

    public override void PhysicsProcess(float delta)
    {
        if(owner.IsGrounded())
        {
            if(move.getDirection() == Vector3.Zero)
            {
                stateMachine.TransitionToState("Move/Idle");
            }
            move.PhysicsProcess(delta);
        }
        else
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("Velocity", move.Velocity);
            stateMachine.TransitionToState("Move/Air", msg);
        }
    }
}
