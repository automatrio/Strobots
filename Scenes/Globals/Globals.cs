using Godot;
using System;

public static class Globals
{
    private static Node _currentPlayerEntity;
    public static Node CurrentPlayerEntity 
    {
        get => _currentPlayerEntity;
        set
        {
            _currentPlayerEntity = value;
            OnPlayerEntityChosen(value);
        } 
    }

    private static GameMode _currentMode = GameMode.PlayMode;
    public static GameMode CurrentMode
    {
        get => _currentMode;
        set
        {
            _currentMode = value;
            GD.Print("Mode set.");
            OnModeChosen();
        } 
    }

    // Old syntax: 
    // public delegate void PlayerEntityHandler(object sender, EventArgs e);
    // public event PlayerEntityHandler PlayerEntityChosen; 
    public static event EventHandler<EventArgs> PlayerEntityChosen;

    public static void OnPlayerEntityChosen(Node node)
    {
        PlayerEntityChosen?.Invoke(node, EventArgs.Empty);
    }

    public static event EventHandler<EventArgs> ModeChosen;

    public static void OnModeChosen()
    {
        ModeChosen?.Invoke(null, EventArgs.Empty);
    }
}

public enum GameMode
{
    MapMode,
    PlayMode
}
