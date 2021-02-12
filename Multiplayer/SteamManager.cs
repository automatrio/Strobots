using Godot;
using System;
using Steamworks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SteamManager : Node
{
    // properties
    public ObservableCollection<string> SteamManagerExceptions { get; set; } = new ObservableCollection<string>();

    public override void _Ready()
    {
        Globals.DebugControlCreated += SanityCheck;
    }

    public void SanityCheck(object sender, EventArgs args)
    {
        // initial sanity check
        if(!Packsize.Test())
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] The packsize returned false; the wrong version of Steam is being used on this platform.");
        }
        if(!DllCheck.Test())
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] The DllCheck test returned false; one or more of the Steamworks binaries might be not up to date.");
        }
        try
        {
            if(SteamAPI.RestartAppIfNecessary((AppId_t)480))
            {
                SteamManagerExceptions.Add("WARNING: [Steamworks.Net] Restarting through Steam");
                GetTree().Quit();
            }
        }
        catch (System.DllNotFoundException ex)
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] Failed to initalize Steamworks.dll");
            SteamManagerExceptions.Add(ex.Message);
        }
        if(SteamAPI.Init())
        {
            SteamManagerExceptions.Add("Steam initialized succesfully!");
            GD.Print("this happened");
        }
        else
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] Failed to initalize Steam. Please make sure the Steam client is open.");
        }
    }
}
