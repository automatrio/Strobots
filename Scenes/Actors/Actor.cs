using Godot;
using System;

public abstract class Actor : KinematicBody
{
    [Export] NodePath StateMachinePath;
    [Export] float mouseSensitivity = 0.003f;
    [Export] float steepAngle = Mathf.Deg2Rad(30);
    [Export] float rotationSpeed = 4;
    private StateMachine stateMachine;
    private Vector3 FLOOR_NORMAL {get;} = Vector3.Up;
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
        if(@event is InputEventMouseMotion eventMouseMotion)
        {
            this.RotateY(-eventMouseMotion.Relative.x * mouseSensitivity);
            pivot.RotateX(-eventMouseMotion.Relative.y * mouseSensitivity);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var collisionNormal = groundingRay.GetCollisionNormal();

        if(groundingRay.IsColliding() && collisionNormal.AngleTo(Vector3.Up) <= steepAngle)
        {
            RotateAlongAxis(collisionNormal, delta);
        }
        else
        {
            RotateAlongAxis(Vector3.Up, delta);
        }
    }

    public bool IsGrounded()
    {
        if(groundingRay.IsColliding() || IsOnFloor())
        {
            return true;
        }
        return false;
    }

    private void RotateAlongAxis(Vector3 axis, float delta)
    {
        Vector3 cross = GlobalTransform.basis.y.Cross(axis);

            if(cross.Length() > 0.001f)
            {
                float angle = GlobalTransform.basis.y.AngleTo(axis);
                Rotate(
                    cross.Normalized(),
                    Mathf.Min(angle, angle * rotationSpeed * delta)
                    );
            }
    }
}
