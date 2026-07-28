using Godot;

public partial class SceneChangeUI : Control
{
    private const string ANIM_FILL = "fill";
    private const string ANIM_EMPTY = "empty";
    public static SceneChangeUI Instance { get; private set; }

    [Signal] public delegate void FilledScreenEventHandler();
    [Signal] public delegate void EmptiedScreenEventHandler();

    [Export] private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        Instance = this;
    }

    public void FillScreen()
    {
        animationPlayer.Play(ANIM_FILL);
    }

    public void EmptyScreen()
    {
        GD.Print("Empty animation");
        animationPlayer.Play(ANIM_EMPTY);
    }

    public void EmitFilledSignal()
    {
        EmitSignalFilledScreen();
    }

    public void EmitEmptiedSignal()
    {

        GD.Print("Empty animation signal");
        EmitSignalEmptiedScreen();
    }
}
