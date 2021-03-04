using Godot;
using System;

public class Player : Actor
{
    public Player()
    {
        AddToGroup("Player");
        Globals.CurrentPlayerEntity = this;
        GD.Print("I'm ready, says player");
    }

    public override void _Ready()
    {
        base._Ready();
    }
}
