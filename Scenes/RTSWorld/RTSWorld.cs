using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class RTSWorld : Node2D
{
    // exports
    [Export] public NodePath FamilyPath;
    [Export] public NodePath MapPath; 

    // fields
    private Rect2 _selectionRectangle;
    private Vector2 _clickPosition;
    private bool _isSelecting;
    private bool _tempCircle;

    // properties
    public Rect2 SelectionRectangle { get; private set; }

    // resources
    private PackedScene robotIcon = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Icons/RobotIcon.tscn");
    private PackedScene playerIcon = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Icons/PlayerIcon.tscn");


    public RTSWorld()
    {
        Globals.FamilyCreated += GenerateIcons;
    }

    public override void _Ready()
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("left_click"))
        {
            if (!_isSelecting)
            {
                _clickPosition = GetLocalMousePosition();
                _selectionRectangle = new Rect2();
                _isSelecting = true;
            }
            if (@event is InputEventMouseMotion && _isSelecting)
            {
                _selectionRectangle.Position = _clickPosition;
                _selectionRectangle.End = GetLocalMousePosition();
                Update();
            }
        }
        else if (Input.IsActionJustReleased("left_click"))
        {
            _selectionRectangle = new Rect2();
            _isSelecting = false;
            Update();
        }

        if (Input.IsActionJustPressed("right_click"))
        {
            if(Globals.CurrentSelection.Count > 0)
            {
                _clickPosition = GetLocalMousePosition();
                Globals.AssignedDestination = GetGlobalMousePosition(); // * new Vector2(1.0f,1.0f)/(Globals.MapToWorldRatio);
                _tempCircle = true;
                Update();
            }
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if(_isSelecting)
        {
            Globals.CurrentSelection = TestForCollisions(_selectionRectangle);
        }
    }
    public override void _Draw()
    {
        DrawRect(_selectionRectangle, new Color(0, .2f, 1, .4f), true);
        DrawRect(_selectionRectangle, new Color(0, .2f, 1, .4f), false);

        if(_tempCircle)
        {
            DrawCircle(_clickPosition, 25.0f, new Color(1,.5f,.5f,.2f));
            DrawCircle(_clickPosition, 20.0f, new Color(1,.5f,.5f,.2f));
            DrawCircle(_clickPosition, 15.0f, new Color(1,.5f,.5f,.2f));
            DrawCircle(_clickPosition, 10.0f, new Color(1,.5f,.5f,.2f));
            DrawCircle(_clickPosition,  5.0f, new Color(1,.5f,.5f,.2f));
        }
    }


    // internal and external methods
    public List<Node> TestForCollisions(Rect2 rectangle)
    {
        List<Node> colliders = new List<Node>();

        var queryParameters = new Physics2DShapeQueryParameters();
        var rectShape = new RectangleShape2D();
        rectShape.Extents = rectangle.Size * 0.5f;
        queryParameters.SetShape(rectShape);
        queryParameters.Transform = new Transform2D(0, rectangle.Position + (rectangle.Size * 0.5f) + Position);

        var collisionResults = GetWorld2d().DirectSpaceState.IntersectShape(queryParameters);

        foreach (Dictionary result in collisionResults)
        {
            colliders.Add(result["collider"] as Node);
        }

        return colliders;
    }
    public void GenerateIcons(object family, EventArgs e)
    {
        foreach(KinematicBody child in (family as Node).GetChildren())
        {
            Icon newIcon = ChooseIcon(child);

            (newIcon as Icon).Master = child as Actor;
            (newIcon as Icon).MapToWorldRatio = Globals.MapToWorldRatio;
            (newIcon as Icon).CanFollow = true;
            (newIcon as Icon).Transform = new Transform2D(0, newIcon.GetMasterCurrentPosition(child as Actor, Globals.MapToWorldRatio));
        }
    }
    public Icon ChooseIcon(KinematicBody node)
    {
        if(node.IsInGroup("Robots"))
        {
            var newIcon = (robotIcon.Instance() as RobotIcon);
            AddChild(newIcon);
            return newIcon as Icon;
        }
        else if(node.IsInGroup("Player"))
        {
            var newIcon = playerIcon.Instance() as PlayerIcon;
            AddChild(newIcon);
            return newIcon as Icon;
        }
        else
        {
            // Improve on this scenario
            var newIcon = robotIcon.Instance() as Icon;
            return newIcon;
        }
    }

}