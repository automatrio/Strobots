using Godot;
using System;

public class Pivot : Spatial
{
    // exports
    [Export] public float ZoomSpeed = 1.0f;

    // fields
    private Transform _originalTransform;
    private bool _canZoomIn;
    private Node _currentZoomObject;
    
    // children
    private Camera camera;
    
    public override void _Ready()
    {
        camera = GetNode<Camera>("Camera");
        _originalTransform = GlobalTransform;
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("left_click") && LobbyGlobals.ObjectHoveredByMouse is Node)
        {
            _canZoomIn = true;
            _currentZoomObject = LobbyGlobals.ObjectHoveredByMouse;
        }
        else if(Input.IsActionJustPressed("left_click") && LobbyGlobals.ObjectHoveredByMouse == null)
        {
            _canZoomIn = false;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 from = camera.ProjectRayOrigin(GetViewport().GetMousePosition());
        Vector3 to = camera.ProjectRayNormal(GetViewport().GetMousePosition()) * 400;

        var result = GetWorld().DirectSpaceState.IntersectRay(from, to);

        if(result.Keys.Count > 0)
        {
            // If there are any objects under the mouse, store them globally so they can be highlighted
            if((result["collider"] as Node).IsInGroup("Highlightables"))
            {
                LobbyGlobals.ObjectHoveredByMouse = result["collider"] as Node;
            }
        }
        else
        {
            // If not, clear the variable
            LobbyGlobals.ObjectHoveredByMouse = null;
        }

        if(_canZoomIn)
        {
            // Zooms in if the user clicks on a valid object
            var zoomedTransform = _currentZoomObject.GetNode<Spatial>("CameraSlot").GlobalTransform;
            
            if(GlobalTransform.origin.DistanceTo(zoomedTransform.origin) > 0.5)
            {
                Transform = Transform.InterpolateWith(zoomedTransform, ZoomSpeed * delta);
            }
        }
        else
        {
            // Zooms out if the user clicks outside of a valid object
            if(GlobalTransform.origin.DistanceTo(_originalTransform.origin) > 0.05)
            {
                Transform = Transform.InterpolateWith(_originalTransform, ZoomSpeed * 4.0f * delta);
            }
        }
        
    }
}