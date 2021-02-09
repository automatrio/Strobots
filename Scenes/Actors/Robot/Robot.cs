using Godot;
using System;

public class Robot : Actor
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    public Robot()
    {
        AddToGroup("Robots");
    }
    public override void _Ready()
    {
        GD.Print("Actor");
        base._Ready();
        GD.Print("Robot");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
