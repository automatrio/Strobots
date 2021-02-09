using Godot;
using System;

public abstract class Icon : KinematicBody2D
{
    protected Sprite _sprite;
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    private IconCommand currentState;
    public IconCommand CurrentState { get => currentState; set => currentState = value; }

    public Vector2 MapToWorldRatio { get; set; }

    private Actor _master;
    public Actor Master { get => _master; set => _master = value; }

    public bool CanFollow = false;
    
    public Icon()
    {

    }
    public async override void _Ready()
    {
        await ToSignal(this, "tree_entered");
        _sprite = GetNode<Sprite>("Sprite");
        (Owner as RTSWorld).SelectionStatus += Highlight;
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
    public void Highlight(object world, SelectionEventArgs args)
    {
        IconCommand command = args.Command;
        if ((world as RTSWorld).CurrentSelection.Contains(this))
            switch (command)
            {
                case IconCommand.Reset:
                    (_sprite.Material as ShaderMaterial).SetShaderParam("outline", false);
                    break;
                case IconCommand.WithinSelection:
                    (_sprite.Material as ShaderMaterial).SetShaderParam("outline", true);
                    (_sprite.Material as ShaderMaterial).SetShaderParam("line_color", new Color(0.0f, .5f, .5f, 1f));
                    break;
                case IconCommand.Active:
                    (_sprite.Material as ShaderMaterial).SetShaderParam("outline", true);
                    (_sprite.Material as ShaderMaterial).SetShaderParam("line_color", new Color(0.0f, 1.0f, .2f, 1f));
                    break;
                default:
                    (_sprite.Material as ShaderMaterial).SetShaderParam("outline", false);
                    break;
            }
        else
        {
            (_sprite.Material as ShaderMaterial).SetShaderParam("outline", false);
        }
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
