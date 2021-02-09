using Godot;
using System.Collections.Generic;
using System;


public class Air : State
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

        if(msg.ContainsKey("Velocity"))
        {
            move.Velocity = (Vector3)msg["Velocity"];
        }
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
        if(!owner.IsGrounded())
        {
            move.Direction += Vector3.Down * move.Gravity * delta;
            move.PhysicsProcess(delta);
        }
        else
        {
            if(move.GetDirection() == Vector3.Zero)
            {
                stateMachine.TransitionToState("Move/Idle");
            }
            else
            {
                stateMachine.TransitionToState("Move/Run");
            }
        }
    }
}
