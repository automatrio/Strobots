using Godot;
using System;
using System.Collections.ObjectModel;

public static class LobbyGlobals
{
    // fields
    private static Node _objectHoveredByMouse;
    private static Node _currentMenuOption;
    private static bool _isReadyToPlay;

    // properties
    public static Node ObjectHoveredByMouse
    {
        get => _objectHoveredByMouse;
        set
        {
            _objectHoveredByMouse = value;
            ObjectUnderMouseCursor?.Invoke(null, EventArgs.Empty);
        }
    }
    public static Node CurrentMenuOption
    {
        get => _currentMenuOption;
        set
        {
            _currentMenuOption = value;
            MenuOptionChosen?.Invoke(_currentMenuOption, EventArgs.Empty);
        }
    }

    public static bool IsReadyToPlay
    { 
        get => _isReadyToPlay;
        set
        { 
            _isReadyToPlay = value;
            ReadyToPlayToggled?.Invoke(null, EventArgs.Empty);
        }
    }

    // signaling
    public static event EventHandler ObjectUnderMouseCursor;
    public static event EventHandler MenuOptionChosen;
    public static event EventHandler ReadyToPlayToggled;
}
