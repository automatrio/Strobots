using Godot;
using System;
using Steamworks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SteamManager : Node
{
    // properties
    public ObservableCollection<string> SteamManagerExceptions { get; set; } = new ObservableCollection<string>();

    // signaling
    public event EventHandler<EventArgs> SteamInitializedSuccesfully;

    public override void _Ready()
    {
        Globals.DebugControlCreated += SanityCheck;
    }

    public void SanityCheck(object sender, EventArgs args)
    {
        // initial sanity check
        if(!Packsize.Test())
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] The packsize returned false; \n the wrong version of Steam is being used on this platform.");
        }
        if(!DllCheck.Test())
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] The DllCheck test returned false; \n one or more of the Steamworks binaries might be not up to date.");
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
        // initaliaze steam
        if(SteamAPI.Init())
        {
            SteamManagerExceptions.Add("Steam initialized succesfully!");
            SteamInitializedSuccesfully?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            SteamManagerExceptions.Add("ERROR: [Steamworks.Net] Failed to initalize Steam. \n Please make sure the Steam client is open.");
        }
    }

    public override void _Process(float delta)
    {
        // callbacks
        SteamAPI.RunCallbacks(); // actually checks for callbacks (events) and execute them
    }
    public Texture GetUserAvatar(CSteamID steamID)
    {
        int friendAvatar = SteamFriends.GetLargeFriendAvatar(steamID); // triggers an AvatarImageLoaded_t callback
        
        uint imageWidth;
        uint imageHeight;
        SteamUtils.GetImageSize(friendAvatar, out imageWidth, out imageHeight);

        byte[] imageArray = new byte[imageWidth * imageHeight * 4]; // a byte array onto which the image is to be stored (4 because rgba)
        var avatarTexture = new ImageTexture();
        bool success = SteamUtils.GetImageRGBA(friendAvatar, imageArray, imageArray.Length); // loads the image onto a buffer
        if (success)
        {
            var image = new Image();
            image.CreateFromData((int)imageWidth, (int)imageHeight, false, Image.Format.Rgba8, imageArray);
            avatarTexture.CreateFromImage(image);
        }

        return avatarTexture as Texture;
    }
}
