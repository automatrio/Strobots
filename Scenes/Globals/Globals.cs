using Godot;
using System;

public static class Globals
{
    public static GameMode CurrentMode { get; set; } = GameMode.PlayMode;
}

public enum GameMode
{
    MapMode,
    PlayMode
}
