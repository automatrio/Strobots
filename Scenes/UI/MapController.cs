using Godot;
using System;
using System.Collections.Generic;

public class MapController : Area2D
{
    [Export] public NodePath IconControlPath;
    private IconControl _iconControl;
    public event EventHandler<EventArgs> MapHoveredByMouse;

    public Rect2 SelectionRectangle { get; set; } // this is just an empty pointer
    private bool _isSelecting;
    public bool IsSelecting
    {
        get => _isSelecting;
        set
        {
            _isSelecting = value;
        }
    }
    private bool _canSelect;
    public bool CanSelect 
    {
        get => _canSelect;
        set{
            _canSelect = value;
            OnMapHoveredByMouse();
        }
    }
    private Vector2 clickPosition;

    private Rect2 tempRect;

    public override void _Ready()
    {
        _iconControl = GetNode<IconControl>(IconControlPath);
    }

    public override void _Input(InputEvent @event)
    {
        if(_canSelect)
        {
            if(Input.IsActionPressed("left_click"))
            {
                if(!IsSelecting)
                {
                    clickPosition = GetLocalMousePosition();
                    IsSelecting = true;
                }
                if(@event is InputEventMouseMotion && IsSelecting)
                {
                    SelectionRectangle = new Rect2(){
                        Position = clickPosition,
                        End = GetLocalMousePosition()
                        };
                    Update();
                }
            }
        }
        if(Input.IsActionJustReleased("left_click") && IsSelecting)
        {
            SelectionRectangle = new Rect2();
            IsSelecting = false;
            Update();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        _iconControl.CheckForCollisions(SelectionRectangle);
    }

    public override void _Draw()
    {
        // base._Draw();
        DrawRect(SelectionRectangle, new Color(0.0f, 0.4f, 1.0f, 0.4f), true);
        DrawRect(SelectionRectangle, new Color(0.0f, 0.4f, 1.0f, 0.4f), false);

        DrawRect(tempRect, new Color(1, 0, 0, 1));
    }

    public void OnMapHoveredByMouse()
    {
        MapHoveredByMouse?.Invoke(this, EventArgs.Empty);
    }
    public void _on_SelectionArea_mouse_entered()
    {
        CanSelect = true;
    }
    public void _on_SelectionArea_mouse_exited()
    {
        CanSelect = false;
    }


}
