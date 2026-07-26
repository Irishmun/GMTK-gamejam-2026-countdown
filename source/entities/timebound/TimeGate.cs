using Godot;

public partial class TimeGate : StaticBody2D
{
    private const string ANIM_RETRACT = "retract";
    private const string ANIM_EXTEND = "extend";

    [Export] private string numberWhenOpen = "5";
    [Export] private AnimationPlayer animationPlayer;

    private bool _open = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalTime.Instance.TimeLeft += Instance_TimeLeft;
        animationPlayer.Play(ANIM_EXTEND);
        _open = false;
    }

    public override void _ExitTree()
    {
        GlobalTime.Instance.TimeLeft -= Instance_TimeLeft;
    }

    private void Instance_TimeLeft(int time)
    {
        if (time.ToString().Contains(numberWhenOpen))
        {
            if (!_open)
            {
                animationPlayer.Play(ANIM_RETRACT);
                _open = true;
            }
        }
        else
        {
            if (_open)
            {
                animationPlayer.Play(ANIM_EXTEND);
                _open = false;
            }
        }
    }
}
