using Godot;
using System;

public class Arrow : KinematicBody
{
    public override void _Ready()
    {
        SetAsToplevel(true);
    }
}
