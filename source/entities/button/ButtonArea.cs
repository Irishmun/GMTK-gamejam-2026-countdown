using Godot;

public partial class ButtonArea : Area2D
{
    private const string ANIM_PRESSED = "pressed";
    private const string ANIM_RELEASED = "released";

    [Signal] public delegate void ButtonPressedEventHandler(string buttonName);
    [Signal] public delegate void ButtonReleasedEventHandler(string buttonName);
    [Signal] public delegate void ButtonActivatedEventHandler(string buttonName);

    [Export] private AnimationPlayer animationPlayer;
    [Export] private string buttonName = "Button";
    [Export] private Sprite2D loadedSprite;
    [Export] private float loadTime = 1;
    [Export] private float unloadTime = 0.25f;


    private int _loadBaseWidth = 128;
    private float _t = 0;
    private bool _fired = false;

    private bool _playerIsOn = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.BodyEntered += ButtonArea_BodyEntered;
        this.BodyExited += ButtonArea_BodyExited;
    }

    private void ButtonArea_BodyExited(Node2D body)
    {
        if (body is not PlayerMovement)
        {
            return;
        }
        _playerIsOn = false;
        animationPlayer.Play(ANIM_RELEASED);
        EmitSignalButtonReleased(buttonName);
    }

    private void ButtonArea_BodyEntered(Node2D body)
    {
        if (body is not PlayerMovement)
        {
            return;
        }
        _playerIsOn = true;
        animationPlayer.Play(ANIM_PRESSED);
        EmitSignalButtonPressed(buttonName);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_playerIsOn)
        {
            _t += (float)delta / loadTime;
        }
        else
        {
            _t -= (float)delta / unloadTime;
            if (_t < 0.5f)
            {
                _fired = false;
            }
        }

        _t = Mathf.Clamp(_t, 0, 1);

        float width = (float)_loadBaseWidth * (float)_t;

        Rect2 rect = loadedSprite.RegionRect;
        Rect2 newRect = new Rect2(rect.Position, width, rect.Size.Y);
        loadedSprite.RegionRect = newRect;
        Vector2 off = loadedSprite.Offset;
        off.X = (-_loadBaseWidth + width) * 0.5f;
        loadedSprite.Offset = off;

        if (_t >= 1 && _playerIsOn && _fired == false)
        {
            EmitSignalButtonActivated(buttonName);
            _fired = true;
        }
    }
}
