using Godot;
using System;

public class UIControl : Control
{
    // exports
    [Export] public NodePath Camera1Path;
    [Export] public NodePath Camera2Path;
    [Export] public NodePath Camera3Path;

    // fields
    private Camera _camera1;
    private Camera _camera2;
    private Camera _camera3;
    private bool canFollowRobots = false;
    private Node _player;

    public UIControl()
    {
        Globals.FamilyCreated += SetupCameras;
    }

    public override void _Ready()
    {
        // Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Input(InputEvent @event)
    {
        if(Globals.CurrentPlayerEntity.IsInGroup("Player"))
        {
            if(Globals.CurrentMode == GameMode.MapMode)
            {
                if(Input.IsActionJustPressed("map"))
                {
                    Globals.CurrentMode = GameMode.PlayMode;
                    Input.SetMouseMode(Input.MouseMode.Captured);
                    this.Visible = false;
                }
            }
            else
            {
                if(Input.IsActionJustPressed("map"))
                {
                    Globals.CurrentMode = GameMode.MapMode;
                    Input.SetMouseMode(Input.MouseMode.Visible);
                    this.Visible = true;
                }
            }
        }
        else
        {
            if(Input.IsActionJustPressed("map"))
            {
                Globals.CurrentMode = GameMode.PlayMode;
                Globals.CurrentPlayerEntity = _player;
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(canFollowRobots)
        {
            _camera1.GlobalTransform = Globals.Family.CurrentPlayerRobots[0].GetNode<Spatial>("Pivot").GlobalTransform;
            _camera2.GlobalTransform = Globals.Family.CurrentPlayerRobots[1].GetNode<Spatial>("Pivot").GlobalTransform;
            _camera3.GlobalTransform = Globals.Family.CurrentPlayerRobots[2].GetNode<Spatial>("Pivot").GlobalTransform;
        }
    }

    public void SetupCameras(object sender, EventArgs args)
    {
        if(!canFollowRobots)
        {
            _camera1 = GetNode<Camera>(Camera1Path);
            _camera2 = GetNode<Camera>(Camera2Path);
            _camera3 = GetNode<Camera>(Camera3Path);

            _player = Globals.CurrentPlayerEntity;

            canFollowRobots = true;
        }
    }

    public void _on_RobotActivateButton_button_down(object[] binds)
    {
        Globals.CurrentPlayerEntity = Globals.Family.CurrentPlayerRobots[0];
        Globals.CurrentMode = GameMode.PlayMode;
        Input.SetMouseMode(Input.MouseMode.Captured);
        this.Visible = false;
    }
}


