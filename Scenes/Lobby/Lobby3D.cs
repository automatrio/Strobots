using Godot;
using System;

public class Lobby3D : Spatial
{
    private PackedScene gameWorld = ResourceLoader.Load<PackedScene>("res://Scenes/World.tscn");
    public override void _Ready()
    {
        MultiplayerGlobals.ReadyToPlayToggled += InitializeGame;
    }

    public void InitializeGame(object sender, EventArgs args)
    {
        GetTree().ChangeScene("res://Scenes/World.tscn");
        this.QueueFree();
    }
}
