using Godot;
using System;

public class RobotIcon : KinematicBody2D
{
    private Rect2 _boundingBox;
    public Rect2 BoundingBox => _boundingBox;

    private Sprite _sprite;
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public Robot RobotAssigned;
    
    private Vector2 direction;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionPressed("left_click"))
        {
            direction = new Vector2(10,0);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(direction * 200 * delta, Vector2.Up);
    }

    public void Highlight(IconCommand command)
    {
        switch (command)
        {
            case IconCommand.Reset:
                (_sprite.Material as ShaderMaterial).SetShaderParam("outline", false);
                break;
            case IconCommand.WithinSelection:
                (_sprite.Material as ShaderMaterial).SetShaderParam("outline", true);
                (_sprite.Material as ShaderMaterial).SetShaderParam("line_color", new Color(0.0f, .5f, .5f, .5f));
                break;
            case IconCommand.Active:
                (_sprite.Material as ShaderMaterial).SetShaderParam("outline", true);
                (_sprite.Material as ShaderMaterial).SetShaderParam("line_color", new Color(0.0f, 1.0f, .2f, .5f));
                break;
            default:
                (_sprite.Material as ShaderMaterial).SetShaderParam("outline", false);
                break;
        }
    }

    public override void _Draw()
    {
        DrawRect(BoundingBox, new Color(1, 1, 1, .4f), true);
    }
}

public enum IconCommand
{
    Reset,
    WithinSelection,
    Active,
    Engaged,
    Destroyed
}
