using Godot;
using System;

public class Map : ViewportContainer
{
    [Export] public NodePath selectionAreaPath;
    private SelectionArea selectionArea;
    private bool isHighlighting;
    private float highlight_transition = 0.0f;
    public override void _Ready()
    {
        selectionArea = GetNode<SelectionArea>(selectionAreaPath);
        selectionArea.MapHoveredByMouse += HighlightMapShader;
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

    public void HighlightMapShader(object sender, EventArgs e)
    {
        GD.Print("HighlightMapShader called.");
        isHighlighting = !isHighlighting;
    }

}
