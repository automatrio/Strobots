using Godot;
using System;

public class Player : Actor
{
    public async override void _Ready()
    {
        base._Ready();
        AddToGroup("Player");

        await ToSignal(GetTree().Root.GetNode("World").GetNode("Overlays/DebugControl"), "ready");
        Globals.CurrentPlayerEntity = this;
    }
}
