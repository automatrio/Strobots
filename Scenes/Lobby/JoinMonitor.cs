using Godot;
using System;
using Steamworks;
using System.Collections.Generic;

public class JoinMonitor : Highlightable
{
    // fields
    private int _currentItem = 0;
    private CSteamID _joinId;
    private SteamManager _steamManager;
    private Dictionary<string, ulong> userIds = new Dictionary<string, ulong>();

    // children
    private ItemList joinScreenList;
    

    // Steam signaling
    protected Callback<LobbyInvite_t> Callback_lobbyInvite;
    protected Callback<LobbyEnter_t> Callback_lobbyEnter;

    public override void _Ready()
    {
        base._Ready();
        _steamManager = GetNode<SteamManager>("/root/SteamManager");
        joinScreenList = GetNode<ItemList>("JoinScreen/Viewport/PanelContainer/MarginContainer/JoinScreenList");
        Callback_lobbyInvite = Callback<LobbyInvite_t>.Create(onLobbyInvite);
        Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(onLobbyEntered);
        joinScreenList.Select(_currentItem);
    }

    public override void _Input(InputEvent @event)
    {
        if(LobbyGlobals.CurrentMenuOption == this)
            if(Input.IsActionJustPressed("ui_up"))
            {
                if(_currentItem > 0) { _currentItem -= 1; }
                joinScreenList.Select(_currentItem);            
            }
            else if(Input.IsActionJustPressed("ui_down"))
            {
                if(_currentItem < joinScreenList.GetItemCount()) { _currentItem += 1; }
                joinScreenList.Select(_currentItem);
            }

            if(Input.IsActionJustPressed("ui_accept"))
            {
                //join the lobby
                string username = joinScreenList.GetItemText(_currentItem);
                _joinId = (CSteamID)userIds[username];
                SteamMatchmaking.JoinLobby(_joinId);
                MultiplayerGlobals.IsReadyToPlay = true;  
            }
        // leavelobby logic to implement
    }
    public override void OnHighlightableClicked(object sender, EventArgs args)
    {
        if((sender as Node) == (this as Node))
        {
            LobbyGlobals.ObjectUnderMouseCursor -= Highlight;
            LobbyGlobals.MenuOptionChosen -= OnHighlightableClicked;
            Reset();
        }

    }

    private void onLobbyInvite(LobbyInvite_t invitation)
    {
        string username = SteamFriends.GetFriendPersonaName((CSteamID)invitation.m_ulSteamIDUser);
        joinScreenList.AddItem(
            username,
            _steamManager.GetUserAvatar((CSteamID)invitation.m_ulSteamIDUser, option: "medium")
        );

        userIds.Add(username, invitation.m_ulSteamIDLobby);
    }

    private void onLobbyEntered(LobbyEnter_t entrance)
    {
        MultiplayerGlobals.LobbyID = (CSteamID)entrance.m_ulSteamIDLobby;
        
        if(MultiplayerGlobals.IsPlayingAsHost)
        {
            MultiplayerGlobals.Player1_ID = SteamUser.GetSteamID();
            MultiplayerGlobals.Player2_ID = SteamMatchmaking.GetLobbyMemberByIndex(MultiplayerGlobals.LobbyID, 1);
        }
        else
        {
            MultiplayerGlobals.Player1_ID = SteamMatchmaking.GetLobbyOwner((CSteamID)entrance.m_ulSteamIDLobby);
            MultiplayerGlobals.Player2_ID = SteamUser.GetSteamID();
        }
    }

}