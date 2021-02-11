using Godot;
using System;

public class PostProcessing : ColorRect
{
    public override void _Ready()
    {
        Globals.PlayerEntityChosen += ActivatePostProcess;
    }

    public void ActivatePostProcess(object sender, EventArgs args)
    {
        if(Globals.CurrentPlayerEntity is Robot)
        {
            Visible = true;
        }
        else
        {
            Visible = false;
        }
    }
}
