using Godot;
using Steamworks;
using System;

public static class MultiplayerGlobals
{
    public static bool IsPlayingAsHost { get; set; }

    public static CSteamID lobbyID { get; set; }
    public static CSteamID player1_ID { get; set; }
    public static CSteamID player2_ID { get; set; }
}
