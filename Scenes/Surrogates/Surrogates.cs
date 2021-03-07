using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Surrogates : Node
{
    // fields
    private KinematicBody[] children = new KinematicBody[4];

    // resources
    private PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Surrogates/Enemy.tscn");
    private PackedScene robotScene = ResourceLoader.Load<PackedScene>("res://Scenes/Surrogates/RobotSurrogate.tscn");
    public override void _Ready()
    {
        PopulateSurrogates();
    }

    public override void _PhysicsProcess(float delta)
    {
        return;
    }

    private void PopulateSurrogates()
    {
        var enemy = enemyScene.Instance() as KinematicBody;
        AddChild(enemy);
        children[0] = enemy;

        // temporary
        // later this information will be sent through the network
        for (int i = 0; i < 3; i++)
        {
            var newRobot = robotScene.Instance() as KinematicBody;
            newRobot.Name = "Robot" + Convert.ToString(i + 1);
            AddChild(newRobot);

            children[i + 1] = newRobot;
        }
    }

    public void UpdateSurrogateData()
    {
        int[] indices = {0, 1, 2, 3};

        Parallel.ForEach(
            indices,
            index =>
            {
                children[index].GlobalTransform = new Transform
                (
                    Quat.Identity,
                    GetParent<NetworkDataExchange>().SurrogateData[index].Position
                );                
            });      
    }
}
