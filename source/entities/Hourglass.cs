using Godot;

public partial class Hourglass : Area2D
{
    [Signal] public delegate void YouWinEventHandler();
    [Export] private AnimationPlayer animationPlayer;

    public void Interact()
    {
        animationPlayer.Play("tip");
    }

    public void StopClock()
    {
        GlobalTime.Instance.StopTimer();
        EmitSignalYouWin();
    }
}
