using Godot;
using Multiplayer;
using Steamworks;
using System;

public static class MultiplayerGlobals
{
    // fields
    private static bool _isReadyToPlay;
    
    // properties
    public static bool IsReadyToPlay
    { 
        get => _isReadyToPlay;
        set
        { 
            _isReadyToPlay = value;
            ReadyToPlayToggled?.Invoke(null, EventArgs.Empty);
        }
    }
    public static bool IsPlayingAsHost { get; set; }

    public static CSteamID LobbyID { get; set; }
    public static CSteamID Player1_ID { get; set; }
    public static CSteamID Player2_ID { get; set; }

    public static event EventHandler ReadyToPlayToggled;
}
