using Godot;
using System;

public abstract class Actor : KinematicBody
{
    // exports
    [Export] public NodePath StateMachinePath;
    [Export] public float MouseSensitivity = 0.003f;
    [Export] public float SteepAngle = Mathf.Deg2Rad(30);
    [Export] public float RotationSpeed = 4;

    // constants
    private Vector3 FLOOR_NORMAL {get;} = Vector3.Up;

    // properties
    public bool[] IsGrounded
    {
        get
        {
            return new bool[2] {groundingRay.IsColliding(), IsOnFloor()};
        }
    }

    // children
    private StateMachine stateMachine;
    private RayCast groundingRay;
    private Spatial pivot;
    private Camera camera;

    public override void _Ready()
    {
        stateMachine = GetNode<StateMachine>(StateMachinePath);
        groundingRay = GetNode<RayCast>("GroundingRay");
        pivot = GetNode<Spatial>("Pivot");
        camera = pivot.GetNode<Camera>("Camera");
    }

    public override void _Input(InputEvent @event)
    {
        // FPS camera
        if(Globals.CurrentPlayerEntity == this && Globals.CurrentMode == GameMode.PlayMode)
        {
            if(@event is InputEventMouseMotion eventMouseMotion)
            {
                this.RotateY(-eventMouseMotion.Relative.x * MouseSensitivity);
                pivot.RotateX(-eventMouseMotion.Relative.y * MouseSensitivity);
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // adjustment to slope
        // TO-DO: transfer the rotation to root bone only
        var collisionNormal = groundingRay.GetCollisionNormal();

        if(groundingRay.IsColliding() && collisionNormal.AngleTo(Vector3.Up) <= SteepAngle)
        {
            RotateAlongAxis(collisionNormal, delta);
        }
        else
        {
            RotateAlongAxis(Vector3.Up, delta);
        }
    }
    private void RotateAlongAxis(Vector3 axis, float delta)
    {
        Vector3 cross = GlobalTransform.basis.y.Cross(axis);

            if(cross.Length() > 0.001f)
            {
                float angle = GlobalTransform.basis.y.AngleTo(axis);
                Rotate(
                    cross.Normalized(),
                    Mathf.Min(angle, angle * RotationSpeed * delta)
                    );
            }
    }
}
