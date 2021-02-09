using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class RTSWorld : Node2D
{
    [Export] public NodePath FamilyPath;
    [Export] public NodePath MapPath; 
    private Family family;

    private Rect2 _selectionRectangle;
    public Rect2 SelectionRectangle { get; private set; }

    private Vector2 clickPosition;
    private bool isSelecting;

    public EventHandler<SelectionEventArgs> SelectionStatus;

    private List<Node> _currentSelection;
    public List<Node> CurrentSelection { get => _currentSelection; private set => _currentSelection = value; }

    private PackedScene robotIcon = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Icons/RobotIcon.tscn");
    private PackedScene playerIcon = ResourceLoader.Load<PackedScene>("res://Scenes/UI/Icons/PlayerIcon.tscn");

    public override void _Ready()
    {
        GD.Print("RTSWorld");

        Globals.FamilyCreated += GenerateIcons;

    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("left_click"))
        {
            if (!isSelecting)
            {
                clickPosition = GetLocalMousePosition();
                _selectionRectangle = new Rect2();
                isSelecting = true;
            }
            if (@event is InputEventMouseMotion && isSelecting)
            {
                _selectionRectangle.Position = clickPosition;
                _selectionRectangle.End = GetLocalMousePosition();
                SetPhysicsProcessInternal(true);
                Update();
            }
        }
        else if (Input.IsActionJustReleased("left_click"))
        {
            _selectionRectangle = new Rect2();
            isSelecting = false;
            SetPhysicsProcessInternal(false);
            Update();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        _currentSelection = TestForCollisions(_selectionRectangle);
    }

    public override void _Draw()
    {
        DrawRect(_selectionRectangle, new Color(0, .2f, 1, .4f), true);
        DrawRect(_selectionRectangle, new Color(0, .2f, 1, .4f), false);
    }

    public List<Node> TestForCollisions(Rect2 rectangle)
    {
        List<Node> colliders = new List<Node>();
        var queryParameters = new Physics2DShapeQueryParameters();
        var rectShape = new RectangleShape2D();
        rectShape.Extents = rectangle.Size * 0.5f;
        queryParameters.SetShape(rectShape);
        queryParameters.Transform = new Transform2D(0, rectangle.Position + (rectangle.Size * 0.5f));

        var collisionResults = GetWorld2d().DirectSpaceState.IntersectShape(queryParameters);

        foreach (Dictionary result in collisionResults)
        {
            colliders.Add(result["collider"] as Node);
            SelectionStatus?.Invoke(this, new SelectionEventArgs() { Command = IconCommand.WithinSelection });
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
        }
    }

    public Icon ChooseIcon(KinematicBody node)
    {
        if(node.IsInGroup("Robots"))
        {
            var newIcon = robotIcon.Instance() as Icon;
            AddChild(newIcon);
            return newIcon as Icon;
        }
        else if(node.IsInGroup("Player"))
        {
            var newIcon = playerIcon.Instance() as Icon;
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

public class SelectionEventArgs : EventArgs
{
    public IconCommand Command { get; set; }
}