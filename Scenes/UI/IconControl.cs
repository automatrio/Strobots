using Godot;
using System;
using System.Collections.Generic;

public class IconControl : Node2D
{
    // Will be replaced in the future, as user will select which robots to play with

    [Export] public NodePath FamilyPath;
    [Export] public NodePath MapPath;
    private Family _family;
    private Map _map;

    ///////// These robots will be added by another node to the list below.
    public List<Robot> Robots = new List<Robot>();
    public List<RobotIcon> RobotIcons = new List<RobotIcon>();

    private PackedScene _robotIcon = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Icons/RobotIcon.tscn");
     

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _family = GetNode<Family>(FamilyPath);
        _map = GetNode<Map>(MapPath);
        float cameraScale = _map.GetNode<Camera>("Viewport/Camera").Size;

        foreach(Robot robot in _family.CurrentPlayerRobots)
        {
            if(robot != null)
            {
                var newIcon = _robotIcon.Instance() as RobotIcon;
                AddChild(newIcon);
                newIcon.Position = new Vector2(robot.GlobalTransform.origin.x, robot.GlobalTransform.origin.z) * (_map.RectSize / cameraScale);
                newIcon.RobotAssigned = robot;
                RobotIcons.Add(newIcon);
            }
        }
    }

    public void CheckForCollisions(Rect2 rectangle)
    {
        var parameters = new Physics2DShapeQueryParameters();
        var rectShape = new RectangleShape2D();
        
        rectShape.Extents = rectangle.Size * 0.5f;
        parameters.SetShape(rectShape);
        parameters.Transform = new Transform2D(0, rectangle.Position + (rectangle.Size * 0.5f));


        var results = GetWorld2d().DirectSpaceState.IntersectShape(parameters);
        GD.Print(results);

        // tempRect.Size = rectShape.Extents * 2;
        // tempRect.Position = parameters.Transform.origin - rectShape.Extents;
    }
}
