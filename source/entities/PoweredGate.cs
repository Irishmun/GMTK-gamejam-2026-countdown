using Godot;

public partial class PoweredGate : StaticBody2D
{
    private const string ANIM_RETRACT = "retract";
    private const string ANIM_EXTEND = "extend";
    [Export] private AnimationPlayer animationPlayer;

    private bool _open = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _open = false;
    }

    private void PowerSource_PowerStateChanged(bool powered)
    {

        if (powered && !_open)
        {
            animationPlayer.Play(ANIM_RETRACT);
            _open = true;
            return;
        }

        if (!powered && _open)
        {
            animationPlayer.Play(ANIM_EXTEND);
            _open = false;
            return;
        }
    }
}
