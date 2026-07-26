using Godot;

public partial class PlayerFollowerCamera : CharacterBody2D
{
    [Export] private Area2D followTrigger;

    private CharacterBody2D _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        followTrigger.BodyEntered += FollowTrigger_BodyEntered;
        followTrigger.BodyExited += FollowTrigger_BodyExited;
    }

    private void FollowTrigger_BodyExited(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }
        _player = null;
    }

    private void FollowTrigger_BodyEntered(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }
        _player = (CharacterBody2D)body;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null)
        {
            Velocity = Vector2.Zero;
        }
        else
        {
            Velocity = _player.Velocity;
        }

        MoveAndSlide();
    }
}
