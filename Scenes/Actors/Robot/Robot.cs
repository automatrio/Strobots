using Godot;
using System;

public class Robot : Actor
{
    // exports 
    [Export] public float Mass = 2.0f;

    // fields
    private Vector3 _destination;
    private Vector3[] _path = new Vector3[0];
    private int _currentPathNode = 0;

    // properties
    public Vector3 Destination
    { 
        get => _destination;
        set
        {
            _destination = value;
            MoveToTarget(_destination);
        }
    }

    // children
    private Navigation navigation;

     // resources
    private PackedScene arrow = ResourceLoader.Load<PackedScene>("res://Scenes/Actors/Robot/Arrow.tscn");


    public Robot()
    {
        AddToGroup("Robots");
    }
    public override void _Ready()
    {
        base._Ready();
        navigation = GetTree().Root.GetNode<Spatial>("World").GetNode<Navigation>("Navigation");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Globals.CurrentPlayerEntity != this)
        {
            if(_currentPathNode < _path.Length)
            {
                if(GlobalTransform.origin.DistanceTo(_path[_currentPathNode]) < 1.0f)
                {
                    _currentPathNode += 1;
                }
                else
                {
                    stateMachine.GetNode<Move>("Move").Direction = FollowTarget(
                        stateMachine.GetNode<Move>("Move").Direction,
                        GlobalTransform.origin,
                        _path[_currentPathNode],
                        Mass
                    );
                    // rotate so as to face the current target
                    var rotatedTransform = Transform.LookingAt(_path[_currentPathNode], GlobalTransform.basis.y);
                    Transform = Transform.InterpolateWith(rotatedTransform, RotationSpeed * delta);

                }
            }
            else
            {
                _destination = Vector3.Zero;
                _currentPathNode = 0;
                _path = new Vector3[0];
                stateMachine.GetNode<Move>("Move").Direction *= new Vector3(0,1,0);
            }
        }
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        _path = navigation.GetSimplePath(GlobalTransform.origin, targetPosition);
        _currentPathNode = 0;
        CreateArrow(targetPosition);
        stateMachine.TransitionToState("Move/Run");
    }

    public void CreateArrow(Vector3 arrowOrigin)
    {
        // if(GetNode<Node>("Arrow") == null)
        // {
            Node newArrow = arrow.Instance();
            AddChild(newArrow);
            (newArrow as Arrow).Transform = new Transform(Basis.Identity, arrowOrigin);
        // }
        // else
        // {
        //     GetNode<Node>("Arrow").QueueFree();
        //     CreateArrow(arrowOrigin);
        // }
    }

    public static Vector3 FollowTarget(
        Vector3 currentDirection,
        Vector3 currentPosition,
        Vector3 targetPosition,
        float mass
    )
    {
        var newDirection = (targetPosition - currentPosition).Normalized();
        var steerDirection = (newDirection - currentDirection) * mass;

        return (currentDirection + steerDirection).Normalized();
    }
}