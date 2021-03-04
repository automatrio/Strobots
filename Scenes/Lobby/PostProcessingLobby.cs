using Godot;
using System;

public class PostProcessingLobby : CanvasLayer
{
    // children
    private AnimationPlayer animationPlayer;

    public async override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        await ToSignal(GetTree().CreateTimer(9.5f), "timeout");
        animationPlayer.Play("FadeInLogo");
        await ToSignal(GetTree().CreateTimer(14f), "timeout");
        animationPlayer.Play("FadeInTitle");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
