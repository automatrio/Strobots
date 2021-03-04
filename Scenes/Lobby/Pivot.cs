using Godot;
using System;

public class Pivot : Spatial
{
    // exports
    [Export] public float ZoomInSpeed = 6.0f;
    [Export] public float ZoomOutSpeed = 0.2f;

    // fields
    private Transform _originalTransform;
    private MenuCameraStatus _menuCameraStatus = MenuCameraStatus.Origin;
    private Node _currentZoomObject;
    private float _distance = 0.0f;
    
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
            _menuCameraStatus = MenuCameraStatus.MoveToTarget;
            _currentZoomObject = LobbyGlobals.ObjectHoveredByMouse;
            LobbyGlobals.CurrentMenuOption = _currentZoomObject;
        }
        else if(Input.IsActionJustPressed("left_click") && LobbyGlobals.ObjectHoveredByMouse == null)
        {
            _menuCameraStatus = MenuCameraStatus.MoveToOrigin;
            LobbyGlobals.CurrentMenuOption = null;
            _distance = GlobalTransform.origin.DistanceTo(_originalTransform.origin);
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

        if(_menuCameraStatus == MenuCameraStatus.MoveToTarget)
        {
            // Zooms in if the user clicks on a valid object
            var zoomedTransform = _currentZoomObject.GetNode<Spatial>("CameraSlot").GlobalTransform;
            
            if(GlobalTransform.origin.DistanceTo(zoomedTransform.origin) > 0.05)
            {
                GlobalTransform = GlobalTransform.InterpolateWith(zoomedTransform, ZoomInSpeed * delta);
            }
        }
        else if (_menuCameraStatus == MenuCameraStatus.MoveToOrigin)
        {
            if(_distance > 0.0f)
            {
                float ratio = 1.0f - (GlobalTransform.origin.DistanceTo(_originalTransform.origin) / _distance);
                GlobalTransform = GlobalTransform.InterpolateWith(_originalTransform, Mathf.Max(0.1f, ratio) * ZoomOutSpeed);
            }
            else
            {
                _menuCameraStatus = MenuCameraStatus.Origin;
            }
        }
        else
        {
            return;
        }
        
    }
}

public enum MenuCameraStatus
{
    Origin,
    MoveToTarget,
    MoveToOrigin

}