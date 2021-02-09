using Godot;
using System;

public class Map : ViewportContainer
{
    private bool isHighlighting;
    private float highlight_transition = 0.0f;
    public float CameraSize { get; set; }
    public override void _Ready()
    {
        GD.Print("Map");
        CameraSize = GetNode<Camera>("Viewport/Camera").Size;
        Globals.MapToWorldRatio = RectSize / CameraSize;
    }

    public override void _Process(float delta)
    {
        if(isHighlighting) 
        {
            var position = new Vector2(GetLocalMousePosition().x/RectSize.x, 1.0f - GetLocalMousePosition().y/RectSize.y);
            (Material as ShaderMaterial).SetShaderParam("mouse_position", position);
            
            highlight_transition = Mathf.Lerp(highlight_transition, 0.4f, 0.25f);
            (Material as ShaderMaterial).SetShaderParam("highlight_intensity", highlight_transition);
        }
        else
        {
            highlight_transition = Mathf.Lerp(highlight_transition, 0.0f, 0.25f);
            (Material as ShaderMaterial).SetShaderParam("highlight_intensity", highlight_transition);
        } 
    }

    // Godot signals
    public void _on_MapArea_mouse_entered()
    {
        isHighlighting = true;
    }

    public void _on_MapArea_mouse_exited()
    {
        isHighlighting = false;
    }
}
