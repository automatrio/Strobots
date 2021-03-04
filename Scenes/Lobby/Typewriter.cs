using Godot;
using System;
using Steamworks;

public class Typewriter : Highlightable
{
    // fields
    protected Callback<LobbyChatUpdate_t> Callback_lobbyChatUpdate;

    // children
    private RichTextLabel chatBox;


    public override void _Ready()
    {
        base._Ready();

        chatBox = GetNode<RichTextLabel>("ChatScreen/Viewport/VBoxContainer/Panel/ChatBox");
        Callback_lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(onChatUpdate);
    }

    public override void OnHighlightableClicked(object sender, EventArgs args)
    {
        if((sender as Node) == (this as Node))
        {
            return;
        }
    }

    private void onChatUpdate(LobbyChatUpdate_t update)
    {
        string action;
        switch(update.m_rgfChatMemberStateChange)
        {
            case 1:
                action = " has joined the lobby.";
                break;
            case 2:
                action = " has left the lobby.";
                break;
            case 3:
                action = " has disconnected without leaving.";
                break;
            case 4:
                action = " has been kicked.";
                break;
            case 5:
                action = " has been kicked or banned.";
                break;
            default:
                action = " has done something. But what?";
                break;
        }
        chatBox.AddText(
            "\n" + SteamFriends.GetFriendPersonaName((CSteamID)update.m_ulSteamIDUserChanged)
            + action);
    }
}
