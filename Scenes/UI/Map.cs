using Godot;
using System;

public class Map : ViewportContainer
{
    // fields
    private bool _isHighlighting;
    private float _highlightTransition = 0.0f;
    
    // properties
    public float CameraSize { get; set; }

    // children
    private Camera camera;
    public override void _Ready()
    {
        camera = GetNode<Camera>("Viewport/Camera");
        CameraSize = camera.Size;
        Globals.MapToWorldRatio = RectSize / CameraSize;
        Globals.DestinationAssigned += CameraRayCast; 
    }

    public override void _Process(float delta)
    {
        if(_isHighlighting) 
        {
            var position = new Vector2(GetLocalMousePosition().x/RectSize.x, 1.0f - GetLocalMousePosition().y/RectSize.y);
            (Material as ShaderMaterial).SetShaderParam("mouse_position", position);
            
            _highlightTransition = Mathf.Lerp(_highlightTransition, 0.4f, 0.25f);
            (Material as ShaderMaterial).SetShaderParam("highlight_intensity", _highlightTransition);
        }
        else
        {
            _highlightTransition = Mathf.Lerp(_highlightTransition, 0.0f, 0.25f);
            (Material as ShaderMaterial).SetShaderParam("highlight_intensity", _highlightTransition);
        } 
    }

    // Godot signals
    public void _on_MapArea_mouse_entered()
    {
        _isHighlighting = true;
    }

    public void _on_MapArea_mouse_exited()
    {
        _isHighlighting = false;
    }

    public void CameraRayCast(object sender, AssignmentEventArgs args)
    {
        Vector3 from = camera.ProjectRayOrigin(Globals.AssignedDestination);
        Vector3 to = from + camera.ProjectRayNormal(Globals.AssignedDestination) * 300.0f;

        var self = new Godot.Collections.Array();
        self.Add(this);

        var collisionResult = camera.GetWorld().DirectSpaceState.IntersectRay(from, to, self);

        var newDestination = (Vector3)collisionResult["position"];
        foreach(Node master in args.IconMasters)
        {
            (master as Robot).Destination = newDestination;
        }
    }
}
