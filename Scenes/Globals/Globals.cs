using Godot;
using System;

public static class Globals
{
    // fields
    private static Node _currentPlayerEntity;
    private static GameMode _currentMode = GameMode.PlayMode;
    private static Vector2 _mapToWorldRatio;
    private static State _currentState;
    private static Node _debugControl;
    private static Family _family;

    // properties
    public static Node CurrentPlayerEntity
    {
        get => _currentPlayerEntity;
        set
        {
            _currentPlayerEntity = value;
            PlayerEntityChosen?.Invoke(null, EventArgs.Empty);
        }
    }
    public static GameMode CurrentMode
    {
        get => _currentMode;
        set
        {
            _currentMode = value;
            ModeChosen?.Invoke(null, EventArgs.Empty);
        }
    }
    public static Vector2 MapToWorldRatio
    {
        get => _mapToWorldRatio;
        set
        {
            _mapToWorldRatio = value;
            MapToWorldRatioChanged?.Invoke(null, EventArgs.Empty);
        }
    }
    public static State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            StateChanged?.Invoke(null, EventArgs.Empty);
        }
    }
    public static Node DebugControl
    {
        get => _debugControl;
        set => _debugControl = value;
    }
    public static Family Family
    { 
        get => _family;
        set
        {
            _family = value; 
            FamilyCreated?.Invoke(_family, EventArgs.Empty);
        }
    }
    // signaling
    public static event EventHandler<EventArgs> PlayerEntityChosen;
    public static event EventHandler<EventArgs> ModeChosen;
    public static event EventHandler<EventArgs> MapToWorldRatioChanged;
    public static event EventHandler<EventArgs> StateChanged;
    public static event EventHandler<EventArgs> FamilyCreated;
}

public enum GameMode
{
    MapMode,
    PlayMode
}
