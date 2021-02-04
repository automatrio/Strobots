using Godot;
using System.Collections.Generic;
using System;

public class Move : State
{
    [Export] public float speed = 1000.0f;
    [Export] public float jumpImpulse = 900.0f;
    [Export] public float gravity = 5.0f;


    public Vector3 Direction { get => _direction; set => _direction = value; }
    private Vector3 _direction;
    public Vector3 Velocity { get => _velocity; set => _velocity = value; }
    private Vector3 _velocity;
    private Actor owner;

    public override void _Ready()
    {
        owner = (Actor)Owner;
    }

    public override void EnterState(Dictionary<string, object> msg)
    {
        return;
    }

    public override void ExitState()
    {
        return;
    }

    public override void LocalInput(InputEvent @event)
    {
        if (owner.IsGrounded())
        {
            _direction = getDirection();
        }
    }

    public override void PhysicsProcess(float delta)
    {
        _velocity = calculateVelocity(_direction, speed, delta);
        owner.MoveAndSlide(_velocity, Vector3.Up);
    }

    public Vector3 calculateVelocity(
        Vector3 direction,
        float speed,
        float delta
    )
    {
        // Somehow introduce the old _velocity;
        
        var _velocity = new Vector3();

        _velocity = direction * speed * delta;

        return _velocity;
    }

    public Vector3 getDirection()
    {
        var inputDirection = new Vector3(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("jump"),
            Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward")
        );

        var finalDirection = -owner.GlobalTransform.basis.z * inputDirection.z;
        finalDirection += owner.GlobalTransform.basis.x * inputDirection.x;
        finalDirection = finalDirection.Normalized();

        return finalDirection;
    }
}
