using Godot;

public partial class RoomCamera : Camera2D
{
    [Export] private PlayerMovement player;

    Vector2 _screenSize;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        UpdatePosition();
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 currentCell = (player.GlobalPosition) / _screenSize;
        GlobalPosition = currentCell * _screenSize;
    }
}
