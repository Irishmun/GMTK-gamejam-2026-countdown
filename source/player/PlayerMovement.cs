using Godot;

public partial class PlayerMovement : CharacterBody2D
{
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_MOVING = "Moving";

    [Export] private float speed = 50.0f;
    [Export] private bool sideScroller = false;
    [Export] private AnimationPlayer animationPlayer;
    [Export] private Sprite2D playerSprite;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");//.Normalized();


        if (direction != Vector2.Zero)
        {
            if (direction.X != 0)
            {
                if (direction.X < 0)
                {
                    playerSprite.FlipH = true;
                }
                else
                {
                    playerSprite.FlipH = false;
                }
            }
            animationPlayer.Play(ANIM_MOVING);
            velocity.X = direction.X * speed;
            velocity.Y = sideScroller ? 0 : direction.Y * speed;
        }
        else
        {
            animationPlayer.Play(ANIM_IDLE);
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Y = sideScroller ? 0 : Mathf.MoveToward(Velocity.Y, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
