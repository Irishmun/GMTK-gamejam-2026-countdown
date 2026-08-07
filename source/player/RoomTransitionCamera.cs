using Godot;

public partial class RoomTransitionCamera : Node
{
    [Export] private Camera2D camera;
    [Export] private Area2D[] areas;
    [Export] private float tweenTime = 0.25f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (camera == null)
        {
            camera = GetViewport().GetCamera2D();
        }

        SubscribeAll();
    }

    private void RoomTransitionCamera_BodyExited(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }

        Vector2 dir = camera.GlobalPosition.DirectionTo(body.GlobalPosition) / camera.GetViewportRect().Size;
        Vector2 pos = camera.GlobalPosition;
        if (dir.Abs().X > dir.Abs().Y)//either left or right
        {
            if (dir.X > 0)//down
            {
                pos.X += 400;
            }
            else
            {
                pos.X -= 400;
            }
        }
        else//either up or down
        {
            if (dir.Y > 0)//down
            {
                pos.Y += 240;
            }
            else
            {
                pos.Y -= 240;
            }
        }
        TweenCameraToPosition(pos);
    }

    private void TweenCameraToPosition(Vector2 globalPosition)
    {
        Tween t = GetTree().CreateTween();
        t.SetEase(Tween.EaseType.Out);
        t.SetTrans(Tween.TransitionType.Cubic);
        t.TweenProperty(camera, "global_position", globalPosition, tweenTime);
    }

    public void SubscribeAll()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].BodyExited += RoomTransitionCamera_BodyExited;
        }
    }

    public void UnSubscribeAll()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].BodyExited -= RoomTransitionCamera_BodyExited;
        }
    }
}
