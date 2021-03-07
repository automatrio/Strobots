using Godot;
using System;
using System.Collections.ObjectModel;

public static class LobbyGlobals
{
    // fields
    private static Node _objectHoveredByMouse;
    private static Node _currentMenuOption;

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

    // signaling
    public static event EventHandler ObjectUnderMouseCursor;
    public static event EventHandler MenuOptionChosen;
}
