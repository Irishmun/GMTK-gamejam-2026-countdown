using Godot;

public partial class StartTimerNode : Node
{
    [Export] private PlayerMovement playerMovement;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerMovement.StopMovement();
        SceneChangeUI.Instance.EmptiedScreen += Instance_EmptiedScreen;
        SceneChangeUI.Instance.EmptyScreen();
    }

    private void Instance_EmptiedScreen()
    {
        SceneChangeUI.Instance.EmptiedScreen -= Instance_EmptiedScreen;
        playerMovement.StartMovement();
        GlobalTime.Instance.StartTimer();
    }

}
