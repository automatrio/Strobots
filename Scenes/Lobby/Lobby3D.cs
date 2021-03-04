using Godot;
using System;

public class Lobby3D : Spatial
{
    private PackedScene gameWorld = ResourceLoader.Load<PackedScene>("res://Scenes/World.tscn");
    public override void _Ready()
    {
        LobbyGlobals.ReadyToPlayToggled += InitializeGame;
    }

    public void InitializeGame(object sender, EventArgs args)
    {
        GD.Print("Initializing world...");
        // var newGameWorld = gameWorld.Instance();
        // GetTree().Root.AddChild(newGameWorld);
        // this.QueueFree();
        GetTree().ChangeScene("res://Scenes/World.tscn");
        this.QueueFree();
    }
}
