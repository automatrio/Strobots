using Godot;
using System.Collections.Generic;
using System;

public class Move : State
{
    // exports
    [Export] public float Speed = 800.0f;
    [Export] public float JumpImpulse = 900.0f;
    [Export] public float Gravity = 5.0f;

    // fields
    private Vector3 _direction;
    private Vector3 _velocity;
    private Actor _owner;
    private Vector3 _oldVelocity;

    // properties
    public Vector3 Direction { get => _direction; set => _direction = value; }
    public Vector3 Velocity { get => _velocity; set => _velocity = value; }
    public Vector3 OldVelocity { get => _oldVelocity; set => _oldVelocity = value; }

    public override void _Ready()
    {
        _owner = (Actor)Owner;
    }

    public override void EnterState(Dictionary<string, object> msg)
    {
        _oldVelocity = Vector3.Zero;
    }

    public override void ExitState()
    {
        return;
    }

    public override void LocalInput(InputEvent @event)
    {
        if(Globals.CurrentPlayerEntity == Owner)
        {
            if(_owner.IsGrounded[0] || _owner.IsGrounded[1])
            {
                _direction = GetDirection();
            }
        }
    }

    public override void PhysicsProcess(float delta)
    {
        _velocity = calculateVelocity(_direction, Speed, delta);
        _owner.MoveAndSlide(_velocity, Vector3.Up);
    }

    public Vector3 calculateVelocity(
        Vector3 direction,
        float speed,
        float delta
    )
    {
        var velocity = new Vector3();

        if(_oldVelocity.Length() <= 0)
        {    
            velocity = direction * speed * delta;
        }
        else
        {
            // velocity = _oldVelocity.LinearInterpolate(direction * speed * delta, .5f);
            
            velocity = (direction * speed * delta + _oldVelocity)/2.0f;

            _oldVelocity -= _velocity * delta;
        }

        return velocity;
    }

    public Vector3 GetDirection()
    {
        var inputDirection = new Vector3(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("jump"),
            Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward")
        );

        var finalDirection = -_owner.GlobalTransform.basis.z * inputDirection.z;
        finalDirection += _owner.GlobalTransform.basis.x * inputDirection.x;
        finalDirection = finalDirection.Normalized();

        return finalDirection;
    }
}
