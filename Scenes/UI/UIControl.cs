using Godot;
using System;

public class UIControl : Control
{
    [Export] NodePath Robot1;
    [Export] NodePath Robot2;
    [Export] NodePath Robot3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Input(InputEvent @event)
    {
        if(Globals.CurrentMode == GameMode.MapMode)
        {
            if(Input.IsActionJustPressed("map"))
            {
                Globals.CurrentMode = GameMode.PlayMode;
                Input.SetMouseMode(Input.MouseMode.Captured);
                this.Visible = false;
            }
        }
        else
        {
            if(Input.IsActionJustPressed("map"))
            {
                Globals.CurrentMode = GameMode.MapMode;
                Input.SetMouseMode(Input.MouseMode.Visible);
                this.Visible = true;
            }
        }
    }
}


