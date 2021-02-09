using Godot;
using System;

public class RobotIcon : Icon
{
    private RTSWorld _owner;
    public override void _Ready()
    {
        base._Ready();
        Globals.SelectionStatus += Highlight;
    }

    public override void Highlight(object sender, SelectionEventArgs args)
    {
        IconCommand command = args.Command;

        _sprite.Material = new ShaderMaterial() { Shader = (_sprite.Material as ShaderMaterial).Shader.Duplicate() as Shader };
        var material = (ShaderMaterial)(_sprite.Material);
        
        if(Globals.CurrentSelection.Contains(this))
        {
            switch (command)
            {
                case IconCommand.Reset:
                    material.SetShaderParam("outline", false);
                    break;
                case IconCommand.WithinSelection:
                    material.SetShaderParam("outline", true);
                    material.SetShaderParam("line_color", new Color(0.0f, .5f, .5f, 1f));
                    break;
                case IconCommand.Active:
                    material.SetShaderParam("outline", true);
                    material.SetShaderParam("line_color", new Color(0.0f, 1.0f, .2f, 1f));
                    break;
                default:
                    material.SetShaderParam("outline", false);
                    break;       
            }
        }
        else
        {
            material.SetShaderParam("outline", false);
        }
    }
}
