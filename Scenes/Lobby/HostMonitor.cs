using Godot;
using System;
using Steamworks;

public class HostMonitor : Highlightable
{
    // fields
    private SteamManager _steamManager;

    // children
    private Control hostScreenPanel;
    private TextureRect profilePicture;
    private Label status;

    // Steam signaling
    protected Callback<LobbyCreated_t> Callback_lobbyCreated;


    public override void _Ready()
    {
        base._Ready();

        hostScreenPanel = GetNode<Control>("HostScreen/Viewport/HostScreenPanel");
        status = GetNode<Label>("HostScreen/Viewport/HostScreenPanel/MarginContainer/HBoxContainer/Status");
        profilePicture = GetNode<TextureRect>("HostScreen/Viewport/HostScreenPanel/MarginContainer/HBoxContainer/ProfilePicture");

        _steamManager = GetNode<SteamManager>("/root/SteamManager");

        // Steam event subscriptions
        Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _steamManager.SteamInitializedSuccesfully += displayProfilePicture;
    }
    public override void OnHighlightableClicked(object sender, EventArgs args)
    {
        if((sender as Node) == (this as Node))
        {
            if(!MultiplayerGlobals.IsPlayingAsHost)
            {
                LobbyGlobals.ObjectUnderMouseCursor -= Highlight;
                Reset();
                CreateNewLobby();
            }
            else
            {
                LobbyGlobals.ObjectUnderMouseCursor += Highlight;
                LeaveLobby();
            }
        }
    }

    public void CreateNewLobby()
    {
        SteamAPICall_t newLobby = SteamMatchmaking.CreateLobby(
            ELobbyType.k_ELobbyTypeFriendsOnly, 2
        );
    }

    private void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(MultiplayerGlobals.LobbyID);
        MultiplayerGlobals.IsPlayingAsHost = false;
        status.Text = "Host a \n new game";
    }

    private void OnLobbyCreated(LobbyCreated_t lobby)
    {
        if(lobby.m_eResult == EResult.k_EResultOK)
        {
            status.Text = "Lobby Id: \n" + lobby.m_ulSteamIDLobby + "\n\n Click here to \n stop hosting";
            MultiplayerGlobals.LobbyID = (CSteamID)lobby.m_ulSteamIDLobby;
            MultiplayerGlobals.IsPlayingAsHost = true;
            MultiplayerGlobals.Player1_ID = SteamUser.GetSteamID();

            // experimental
            SteamMatchmaking.InviteUserToLobby((CSteamID)lobby.m_ulSteamIDLobby, SteamUser.GetSteamID());

        }
        else
        {
            status.Text = "Error " + lobby.m_eResult;
        }
    }

    private void displayProfilePicture(object sender, EventArgs args)
    {
        status.Text = "Welcome, agent " + SteamFriends.GetPersonaName();
        profilePicture.Texture = _steamManager.GetUserAvatar(SteamUser.GetSteamID()) as ImageTexture;
    }
}
