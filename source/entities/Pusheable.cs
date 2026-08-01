using Godot;

public partial class Pusheable : CharacterBody2D
{
    private float _yPos;

    public override void _Ready()
    {
        _yPos = GlobalPosition.Y;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            MoveAndSlide();
            Velocity = Vector2.Zero;
            Vector2 pos = GlobalPosition;
            pos.Y = _yPos;
            GlobalPosition = pos;
        }
    }

    public void TryPush(float velocity)
    {
        Velocity = new Vector2(velocity, 0);
    }

}
