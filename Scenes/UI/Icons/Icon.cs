using Godot;
using System;

public abstract class Icon : KinematicBody2D
{
    protected Sprite _sprite;
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public Vector2 MapToWorldRatio { get; set; }

    private Actor _master;
    public Actor Master { get => _master; set => _master = value; }

    public bool CanFollow = false;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (CanFollow)
        {
            MoveAndSlide(GetMasterCurrentPosition(_master, MapToWorldRatio) - Transform.origin, Vector2.Up);
        }
    }

    public Vector2 GetMasterCurrentPosition(Actor master, Vector2 ratio)
    {
        return new Vector2(master.Transform.origin.x, master.Transform.origin.z) * ratio;
    }
    public abstract void Highlight(object sender, SelectionEventArgs args);
}

public enum IconCommand
{
    Reset,
    WithinSelection,
    Active,
    Engaged,
    Destroyed
}
