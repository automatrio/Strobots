using Godot;
using System;
using Steamworks;

public class HostMonitor : Highlightable
{
    // fields
    private bool _isHosting = false;
    private CSteamID lobbyID;

    // children
    private Control hostScreenPanel;

    // Steam signaling
    protected Callback<LobbyCreated_t> Callback_lobbyCreated;


    public override void _Ready()
    {
        base._Ready();

        hostScreenPanel = GetNode<Control>("HostScreen/Viewport/HostScreenPanel");

        // Steam event subscriptions
        Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
    }
    public override void PerformActionWhenClicked(object sender, EventArgs args)
    {
        if((sender as Node) == (this as Node))
        {
            if(!_isHosting)
            {
                LobbyGlobals.ObjectUnderMouseCursor -= Highlight;
                CreateNewLobby();
            }
            else
            {
                DeleteExistingLobby();
            }
        }
    }

    public void CreateNewLobby()
    {
        SteamAPICall_t newLobby = SteamMatchmaking.CreateLobby(
            ELobbyType.k_ELobbyTypeFriendsOnly, 2
        );
    }

    private void DeleteExistingLobby()
    {
        SteamMatchmaking.LeaveLobby(lobbyID);
        _isHosting = false;
        Label status = hostScreenPanel.GetNode<Label>("Status");
        status.Text = "Host a \n new game";
    }

    private void OnLobbyCreated(LobbyCreated_t lobby)
    {
        Label status = hostScreenPanel.GetNode<Label>("Status");

        if(lobby.m_eResult == EResult.k_EResultOK)
        {
            status.Text = "Lobby Id: \n" + lobby.m_ulSteamIDLobby + "\n Click here to \n stop hosting"; 
            _isHosting = true;
            lobbyID = (CSteamID)lobby.m_ulSteamIDLobby;
            MultiplayerGlobals.IsPlayingAsHost = true;
        }
        else
        {
            status.Text = "Error " + lobby.m_eResult;
        }
    }
}
