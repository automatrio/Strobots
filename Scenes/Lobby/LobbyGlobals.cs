using Godot;
using System;
using System.Collections.ObjectModel;

public static class LobbyGlobals
{
    // fields
    private static Node _objectHoveredByMouse;
    
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

    // signaling
    public static event EventHandler ObjectUnderMouseCursor;
}
