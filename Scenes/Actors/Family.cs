using Godot;
using System;
using System.Collections.Generic;

public class Family : Node
{
    // properties
    public List<Robot> CurrentPlayerRobots { get; set; } = new List<Robot>();

    // children
    private KinematicBody player;

    // resources
    private PackedScene robot = ResourceLoader.Load<PackedScene>("res://Scenes/Actors/Robot/Robot.tscn");
    
    public override void _Ready()
    {
        var rand = new Random();
        player = GetNode<KinematicBody>("Player");

        // temporary instantiation of robots, will be replaced
        for (int i = 0; i < 3; i++)
        {
            var newRobot = robot.Instance();
            newRobot.Name = String.Concat("Robot", i + 1);
            AddChild(newRobot);
            // temp:
            
            (newRobot as Robot).Translation = new Vector3((float)rand.NextDouble() * 100, 5, (float)rand.NextDouble() * 100);
            CurrentPlayerRobots.Add(newRobot as Robot);
        }

        Globals.Family = this;
    }

}
