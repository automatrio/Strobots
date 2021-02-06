using Godot;
using System;
using System.Collections.Generic;

public class Family : Node
{
    private PackedScene robot = ResourceLoader.Load<PackedScene>("res://Scenes/Actors/Robot/Robot.tscn");
    public List<Robot> CurrentPlayerRobots = new List<Robot>();
    public override void _Ready()
    {
        for (int i = 0; i < 3; i++)
        {
            var newRobot = robot.Instance();
            newRobot.Name = String.Concat("Robot", i + 1);
            AddChild(newRobot);
            // temp:
            var rand = new Random();
            (newRobot as Robot).Translation = new Vector3((float)rand.NextDouble() * 100, 5, (float)rand.NextDouble() * 100);
            CurrentPlayerRobots.Add(newRobot as Robot);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
