using Godot;
using System;

public class JoinMonitor : Highlightable
{
    public override void PerformActionWhenClicked(object sender, EventArgs args)
    {
        if((sender as Node) == (this as Node))
        {
            GD.Print("Hoorray!");
            LobbyGlobals.ObjectUnderMouseCursor -= Highlight;
            LobbyGlobals.MenuOptionChosen -= PerformActionWhenClicked;
        }
    }
}
