using Godot;
using System;

public abstract class Highlightable : StaticBody
{
    // children
    private MeshInstance meshInstance;
    private Resource highlightShader = ResourceLoader.Load<Resource>("res://Resources/Internal/Shaders/Highlight.shader");

    public Highlightable()
    {
        AddToGroup("Highlightables");
    }

    public override void _Ready()
    {
        meshInstance = GetNode<MeshInstance>("MeshInstance");
        LobbyGlobals.ObjectUnderMouseCursor += Highlight;
    }

    public void Highlight(object sender, EventArgs args)
    {
        if(LobbyGlobals.ObjectHoveredByMouse == (this as Node))
        {
            meshInstance.GetSurfaceMaterial(0).NextPass = new ShaderMaterial()
            {
                Shader = (Shader)highlightShader.Duplicate()
            };
        }
        else
        {
            meshInstance.GetSurfaceMaterial(0).NextPass = null;
        }
    }

    public abstract void PerformActionWhenClicked();
}
