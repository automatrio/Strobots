using Godot;
using System;

public class Player : Actor
{
    public Player()
    {
        AddToGroup("Player");
        Globals.CurrentPlayerEntity = this;
    }

    public override void _Ready()
    {
        base._Ready();
    }
}
