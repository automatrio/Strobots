using Godot;
using System;

public class JoinScreen : MeshInstance
{

    public override void _Ready()
    {
        var texture = (GetNode("Viewport") as Viewport).GetTexture();
        (GetSurfaceMaterial(0) as ShaderMaterial).SetShaderParam("texture_albedo", texture);
    }

}
